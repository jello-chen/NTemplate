using NTemplate.Engine.Aspx.Generator;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;

namespace NTemplate.Engine.Aspx.Parser.SyntaxTree
{
    public class AspxTemplateParser : TemplateParser
    {
        // CodeTemplate directive attributes
        private string language = "C#";
        private string targetLanguage = "C#";
        private string inherits = "NTemplate.TemplateBase";
        private string src = ""; // No code behind

        // Blocks of code to be inserted at the class member level due to <script> tags
        private List<string> classMemberCodeBlocks = new List<string>();

        // Assemblie and Import directives
        protected List<string> assemblies = new List<string>();
        protected List<string> imports = new List<string>();
        protected List<Property> properties = new List<Property>();

        // Statement generators
        private List<StatementGenerator> statementGenerators = new List<StatementGenerator>();

        /*
			This is a Regex based parser that works by scanning the input text for 
			the following patterns: 

				<%@ ... %>                                - Template Directive
				<%= ... %>                                - Expression in Language
				<% ... %>                                 - Block of statements in Language
				<%--... --%>                              - Template Comment
				<!-- #include file="CommonScript.cs" -->  - Include file processing
				<script ...> </script>                    - Code block
				% is escaped by % (so you can output "<% %>" by writing "<%% %%>" in your template)
		
			All other input text is literal text to be copied to the output

			Regex patterns are "compiled", so it helps performance a bit to make them static:
		*/
        protected static RegexOptions _regexOptions = RegexOptions.IgnoreCase | RegexOptions.Multiline | RegexOptions.Singleline;
        protected static Regex _tagRegex = new Regex(@"(?<tag>(?:<%[^%]%>)|(?:<%[^%].*?[^%]%>))", _regexOptions);
        protected static Regex _directiveRegex = new Regex(@"<%@(?<directive>.*?[^%])%>", _regexOptions);
        protected static Regex _expressionRegex = new Regex(@"<%=(?<expression>.*?[^%])%>", _regexOptions);
        protected static Regex _blockRegex = new Regex(@"<%(?!--)(?<block>(?:[^@]|[^=]|[^%])|(?:(?:[^@]|[^=]|[^%]).*?[^%]))%>", _regexOptions);
        protected static Regex _commentRegex = new Regex(@"<%--(?<comment>.*?)--%>", _regexOptions);
        protected static Regex _includeRegex = new Regex(@"<!--\s+\#include\s+file=\""(?<file>[^\""]+)""\s+-->", _regexOptions);
        protected static Regex _scriptRegex = new Regex(@"<script(?<attributes>.*?)>(?<script>.*?)</script>", _regexOptions);

        protected static Regex _attributesParser = new Regex(@"\s*((?<name>.*?)\s*=\s*\""(?<value>.*?)\""\s*){0,}", _regexOptions);
        protected static Regex _directiveParser = new Regex(@"\s*(?<directive>\S+)\s+((?<name>.*?)\s*=\s*\""(?<value>.*?)\""\s*){0,}", _regexOptions);

        public override TemplateParseResult ParseTemplate(string template)
        {
            Preprocess(template);
            return ParseTemplateImpl();
        }

        private void Preprocess(string template)
        {
            // Includes are expanded, just like a preprocessor
            template = ProcessIncludes(template);

            // Get <script> blocks with runat="template" out of the way
            template = ProcessScripts(template);

            // The text between tags is literal text
            int iStartLiteral = 0;
            foreach (Match tagMatch in _tagRegex.Matches(template))
            {
                if (tagMatch.Index > iStartLiteral)
                {
                    string literalText = template.Substring(iStartLiteral, tagMatch.Index - iStartLiteral);
                    ProcessLiteral(literalText);
                }

                string tagText = tagMatch.Groups["tag"].Value;

                if (_directiveRegex.IsMatch(tagText))
                    ProcessDirective(tagText);
                else if (_commentRegex.IsMatch(tagText))
                    ProcessComment(tagText);
                else if (_expressionRegex.IsMatch(tagText))
                    ProcessExpression(tagText);
                else if (_blockRegex.IsMatch(tagText))
                    ProcessBlock(tagText);
                else
                    throw new ApplicationException(string.Format("Tag does not match any tag patterns: {0}", tagText.Substring(0, 60)));

                iStartLiteral = tagMatch.Index + tagMatch.Length;
            }

            if (iStartLiteral < template.Length)
            {
                string literalText = template.Substring(iStartLiteral, template.Length - iStartLiteral);
                ProcessLiteral(literalText);
            }

            OptimizeNewlines();
        }

        private TemplateParseResult ParseTemplateImpl()
        {
            // Use CodeCompileUnit to create a CodeDom tree for the template. Once the tree is created,
            // we let the CodeDomProvider generate source code for the template's assembly

            string templateNamespace = GetCompiledTemplateNamespace();
            string templateClass = GetCompiledTemplateClass();
            string templateMethodName = GetCompiledTemplateMethodName();

            // Module -----------------

            CodeCompileUnit compileUnit = new CodeCompileUnit();

            foreach (string assembly in assemblies)
                compileUnit.ReferencedAssemblies.Add(assembly);

            CodeNamespace ns = new CodeNamespace(templateNamespace);
            compileUnit.Namespaces.Add(ns);
            ns.Imports.Add(new CodeNamespaceImport("System"));
            ns.Imports.Add(new CodeNamespaceImport("System.IO"));
            ns.Imports.Add(new CodeNamespaceImport("System.Text"));
            ns.Imports.Add(new CodeNamespaceImport("System.Collections.Generic"));
            ns.Imports.Add(new CodeNamespaceImport("NTemplate"));
            foreach (string import in imports)
                ns.Imports.Add(new CodeNamespaceImport(import));

            // Class ------------------
            CodeTypeDeclaration cls = new CodeTypeDeclaration(templateClass);
            cls.BaseTypes.Add(new CodeTypeReference(inherits));
            ns.Types.Add(cls);

            // Class constructor
            CodeConstructor defaultConstructor = new CodeConstructor();
            defaultConstructor.Attributes = MemberAttributes.Public;
            cls.Members.Add(defaultConstructor);

            // Class properties
            foreach (Property property in properties)
            {
                string fieldName = "_" + property.Name.Substring(0, 1).ToLower() + property.Name.Substring(1);

                CodeMemberField memberField = new CodeMemberField(property.Type, fieldName);
                cls.Members.Add(memberField);

                CodeMemberProperty memberProperty = new CodeMemberProperty();
                memberProperty.Attributes = MemberAttributes.Public | MemberAttributes.Final;
                memberProperty.Type = new CodeTypeReference(property.Type);
                memberProperty.Name = property.Name;
                memberProperty.HasGet = true;
                memberProperty.HasSet = true;

                // get { return this.< insert fieldname here>; }
                CodeExpression fieldExpression = new CodeFieldReferenceExpression(new CodeThisReferenceExpression(), fieldName);
                memberProperty.GetStatements.Add(new CodeMethodReturnStatement(fieldExpression));

                // set { <field> = value; }
                memberProperty.SetStatements.Add(new CodeAssignStatement(fieldExpression, new CodePropertySetValueReferenceExpression()));

                cls.Members.Add(memberProperty);
            }

            // Overrides `Execute` Method

            CodeMemberMethod generateCode = new CodeMemberMethod();
            generateCode.Name = templateMethodName;
            generateCode.Attributes = MemberAttributes.Public | MemberAttributes.Override;

            //// StringBuilder generatedCode = new StringBuilder();
            //CodeObjectCreateExpression resultNewExpression = new CodeObjectCreateExpression(
            //    "System.Text.StringBuilder", new CodeExpression[] { });
            //CodeVariableDeclarationStatement resultVaribleStatement = new CodeVariableDeclarationStatement(
            //    typeof(StringBuilder), TemplateCompilerSettings.ResultStringBuilderName, resultNewExpression);

            //generateCode.Statements.Add(resultVaribleStatement);

            foreach (StatementGenerator generator in statementGenerators)
            {
                CodeStatement statement = generator.GenerateStatement();
                if (statement != null)
                    generateCode.Statements.Add(statement);
            }

            //CodeExpression builderObject = new CodeVariableReferenceExpression();
            //CodeExpression toStringExpression = new CodeMethodInvokeExpression(builderObject, "Generate", new CodeExpression[] {  });
            //generateCode.Statements.Add(new CodeMethodReturnStatement(toStringExpression));

            cls.Members.Add(generateCode);

            // Add blocks of code due to <script> tags and code behind files
            foreach (string codeBlockText in classMemberCodeBlocks)
            {
                CodeSnippetTypeMember codeBlock = new CodeSnippetTypeMember(codeBlockText);
                cls.Members.Add(codeBlock);
            }

            return new TemplateParseResult
            {
                CodeCompileUnit = compileUnit,
                Namespace = templateNamespace,
                Class = templateClass
            };
        }

        /// <summary>
		/// If a line only has a directive or block statement, then including it in the output
		/// will result in an empty line (possibly with some white space). To make the output
		/// cleaner, we need to suppress these empty lines. To suppress the empty lines, search
		/// through StatementGenerators looking for lines that consist of only white space,
		/// directives and blocks. When we find such a line, we delete all the literals, thus
		/// deleting the white space and the newline.
		/// </summary>
		protected void OptimizeNewlines()
        {
            int iLineStart = 0;
            for (int i = 0; i < statementGenerators.Count; i++)
            {
                if (statementGenerators[i].GetType() == typeof(LiteralGenerator))
                {
                    LiteralGenerator generatorLiteral = (LiteralGenerator)statementGenerators[i];
                    if (generatorLiteral.Text.Contains("\r\n"))
                    {
                        int iLineEnd = i; // Current "line" runs from iLineStartGenerator to iLineEndGenerator
                        if (iLineStart == iLineEnd)
                        {
                            iLineStart++;
                            continue;
                        }
                        bool suppressLine = true;
                        for (int iCheckGenerator = iLineStart; iCheckGenerator <= iLineEnd; iCheckGenerator++)
                        {
                            if (statementGenerators[iCheckGenerator].GetType() == typeof(ExpressionGenerator))
                            {
                                // Expressions always cause output
                                suppressLine = false;
                                break;
                            }
                            else if (statementGenerators[iCheckGenerator].GetType() == typeof(LiteralGenerator))
                            {
                                LiteralGenerator checkLiteral = (LiteralGenerator)statementGenerators[iCheckGenerator];
                                if (checkLiteral.Text.Trim() != "")
                                {
                                    suppressLine = false;
                                    break;
                                }
                            }
                        }
                        if (suppressLine)
                        {
                            // All the literals between iLineStartGenerator and iLineEndGenerator are all white space, so delete them
                            for (int iDeleteGenerator = iLineEnd; iDeleteGenerator >= iLineStart; iDeleteGenerator--)
                            {
                                if (statementGenerators[iDeleteGenerator].GetType() == typeof(LiteralGenerator))
                                {
                                    statementGenerators.RemoveAt(iDeleteGenerator);
                                    iLineEnd--;
                                }
                            }
                        }
                        iLineStart = iLineEnd + 1;
                        i = iLineEnd;
                    }
                }
            }
        }

        protected string ProcessIncludes(string template)
        {
            Match includeMatch = _includeRegex.Match(template);
            while (includeMatch.Success)
            {
                string includeFile = includeMatch.Groups["file"].Value;
                using (StreamReader includeReader = new StreamReader(includeFile))
                {
                    string includeText = includeReader.ReadToEnd();
                    includeText = ProcessIncludes(includeText);
                    template = template.Remove(includeMatch.Index, includeMatch.Length);
                    template = template.Insert(includeMatch.Index, includeText);

                    includeMatch = _includeRegex.Match(template);
                }

            }
            return template;
        }

        protected string ProcessScripts(string template)
        {
            Match scriptMatch = _scriptRegex.Match(template);
            while (scriptMatch.Success)
            {
                string attributesText = scriptMatch.Groups["attributes"].Value;
                Match attributesMatch = _attributesParser.Match(attributesText);
                if (!attributesMatch.Success)
                {
                    scriptMatch = _scriptRegex.Match(template, scriptMatch.Index + scriptMatch.Length);
                    continue;
                }

                Dictionary<string, string> attributes = ConvertAttributes(attributesMatch);
                if (!attributes.ContainsKey("runat") || attributes["runat"] != "template")
                {
                    scriptMatch = _scriptRegex.Match(template, scriptMatch.Index + scriptMatch.Length);
                    continue;
                }

                string scriptText = scriptMatch.Groups["script"].Value;
                classMemberCodeBlocks.Add(scriptText);

                template = template.Remove(scriptMatch.Index, scriptMatch.Length);

                scriptMatch = _scriptRegex.Match(template, scriptMatch.Index);
            }

            return template;
        }

        // Move parsed attributes from a Match into a Dictionary
        protected Dictionary<string, string> ConvertAttributes(Match match)
        {
            Dictionary<string, string> attributes = new Dictionary<string, string>();

            Group attributeNames = match.Groups["name"];
            Group attributeValues = match.Groups["value"];
            for (int iAttribute = 0; iAttribute < attributeNames.Captures.Count; iAttribute++)
                attributes.Add(attributeNames.Captures[iAttribute].Value, attributeValues.Captures[iAttribute].Value);

            return attributes;
        }

        /// <summary>
		/// Each line goes into its own generator object. This makes the generated code a bit nicer
		/// and it makes newline clean up (done later in OptimizeNewlines) easier. We also handle escape 
		/// sequences here.
		/// </summary>
		protected void ProcessLiteral(string literalText)
        {
            literalText = literalText.Replace("<%%", "<%");
            literalText = literalText.Replace("%%>", "%>");
            int iStart = 0;
            int iNewline = literalText.IndexOf("\r\n");
            while (iNewline >= 0)
            {
                int iCount = iNewline - iStart + 2;
                statementGenerators.Add(new LiteralGenerator(literalText.Substring(iStart, iCount)));
                iStart += iCount;
                if (iStart >= literalText.Length)
                    break;
                iNewline = literalText.IndexOf("\r\n", iStart);
            }
            if (literalText.Length > iStart)
                statementGenerators.Add(new LiteralGenerator(literalText.Substring(iStart, literalText.Length - iStart)));
        }

        protected void ProcessDirective(string tagText)
        {
            string directiveText = _directiveRegex.Match(tagText).Groups["directive"].Value;
            Match directiveMatch = _directiveParser.Match(directiveText);
            if (!directiveMatch.Success)
                throw new ApplicationException(string.Format("Cannot parse directive tag: {0}", tagText));
            string directive = directiveMatch.Groups["directive"].Value;
            Dictionary<string, string> attributes = ConvertAttributes(directiveMatch);
            statementGenerators.Add(new DirectiveGenerator(directive, attributes));

            switch (directive)
            {
                case "CodeTemplate":
                    {
                        if (attributes.ContainsKey("Language")) language = attributes["Language"];
                        if (attributes.ContainsKey("TargetLanguage")) targetLanguage = attributes["TargetLanguage"];
                        if (attributes.ContainsKey("Inherits")) inherits = attributes["Inherits"];
                        if (attributes.ContainsKey("Src")) src = attributes["Src"];
                        break;
                    }
                case "Assembly":
                    {
                        if (!attributes.ContainsKey("Name"))
                            throw new ApplicationException(string.Format("Assembly directive requries a Name attribute: {0}", tagText.Substring(0, 60)));
                        assemblies.Add(attributes["Name"]);
                        break;
                    }
                case "Import":
                    {
                        if (!attributes.ContainsKey("Namespace"))
                            throw new ApplicationException(string.Format("Import directive requries a Name attribute: {0}", tagText.Substring(0, 60)));
                        imports.Add(attributes["Namespace"]);
                        break;
                    }
                case "Property":
                    {
                        if (!attributes.ContainsKey("Name") || !attributes.ContainsKey("Type"))
                            throw new ApplicationException(string.Format("Property directive requries Name and Type attributes: {0}", tagText.Substring(0, 60)));

                        properties.Add(new Property(attributes["Name"], attributes["Type"]));
                        break;
                    }
            }
        }

        protected void ProcessComment(string tagText)
        {
            string commentText = _commentRegex.Match(tagText).Groups["comment"].Value;
            statementGenerators.Add(new CommentGenerator(commentText));
        }

        protected void ProcessExpression(string tagText)
        {
            string expressionText = _expressionRegex.Match(tagText).Groups["expression"].Value;
            statementGenerators.Add(new ExpressionGenerator(expressionText));
        }

        protected void ProcessBlock(string tagText)
        {
            string blockText = _blockRegex.Match(tagText).Groups["block"].Value;
            statementGenerators.Add(new BlockGenerator(blockText));
        }

        // Property directive attributes
        protected class Property
        {
            public string Name = "";
            public string Type = "";

            public Property(string name, string type)
            {
                Name = name;
                Type = type;
            }
        }
    }
}
