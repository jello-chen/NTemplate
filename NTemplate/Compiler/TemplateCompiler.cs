using Microsoft.CSharp;
using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;

namespace NTemplate
{
    public class TemplateCompiler : ITemplateCompiler, IDisposable
    {
        private readonly ITemplateParser parser;
        private readonly TextWriter debugOutput;

        public TemplateCompiler(ITemplateParser parser) : this(parser, null)
        {

        }

        public TemplateCompiler(ITemplateParser parser, TextWriter debugOutput)
        {
            this.parser = parser;
            this.debugOutput = debugOutput;
        }

        public virtual TemplateCompileResult CompileTemplate(string template)
        {
            var parseContext = parser.ParseTemplate(template);

            if (debugOutput != null)
                GenerateDebugInfo(parseContext.CodeCompileUnit);

            var assembly = GenerateAssembly(parseContext);
            return new TemplateCompileResult
            {
                CompiledAssembly = assembly,
                Namespace = parseContext.Namespace,
                Class = parseContext.Class,
            };
        }

        protected virtual Assembly GenerateAssembly(TemplateParseResult parseContext)
        {
            using (CodeDomProvider provider = CodeDomProvider.CreateProvider("C#"))
            {
                var compiler = new CompilerParameters();
                compiler.ReferencedAssemblies.Add("System.dll");
                compiler.ReferencedAssemblies.Add("System.Core.dll");
                compiler.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
                compiler.ReferencedAssemblies.Add("NTemplate.dll");
                compiler.GenerateInMemory = true;

                var result = provider.CompileAssemblyFromDom(compiler, parseContext.CodeCompileUnit);
                if (result.Errors.HasErrors)
                {
                    var error = result.Errors.OfType<CompilerError>().Where(i => !i.IsWarning).FirstOrDefault();
                    if (error != null) throw new Exception(error.ErrorText);
                }
                return result.CompiledAssembly;
            }
        }

        private void GenerateDebugInfo(CodeCompileUnit compileUnit)
        {
            using (CSharpCodeProvider codeProvider = new CSharpCodeProvider())
            {
                CodeGeneratorOptions options = new CodeGeneratorOptions();
                options.BracingStyle = "C";

                using (IndentedTextWriter indentwriter = new IndentedTextWriter(debugOutput, "    "))
                {
                    codeProvider.GenerateCodeFromCompileUnit(compileUnit, indentwriter, options);
                    indentwriter.Flush();
                    indentwriter.Close();
                }
            }
        }

        public void Dispose()
        {
            if (debugOutput != null)
                debugOutput.Dispose();
        }
    }
}
