using System.IO;
using System.Web.Razor;
using System.Web.Razor.Generator;

namespace NTemplate.Engine.Razor
{
    public class RazorTemplateParser : TemplateParser
    {
        public override TemplateParseResult ParseTemplate(string template)
        {
            string templateNamespace = GetCompiledTemplateNamespace();
            string templateClass = GetCompiledTemplateClass();
            string templateMethodName = GetCompiledTemplateMethodName();

            var host = new RazorEngineHost(new CSharpRazorCodeLanguage());
            host.DefaultBaseClass = typeof(TemplateBase).FullName;
            host.DefaultNamespace = templateNamespace;
            host.DefaultClassName = templateClass;
            host.NamespaceImports.Add("System");
            host.GeneratedClassContext = new GeneratedClassContext("Execute", "Write", "WriteLiteral")
            {
                WriteAttributeMethodName = "WriteAttribute"
            };

            GeneratorResults generatorResults;
            var engine = new System.Web.Razor.RazorTemplateEngine(host);
            using (var reader = new StringReader(template))
            {
                generatorResults = engine.GenerateCode(reader);
            }
            return new TemplateParseResult
            {
                CodeCompileUnit = generatorResults.GeneratedCode,
                Namespace = templateNamespace,
                Class = templateClass
            };
        }
    }
}
