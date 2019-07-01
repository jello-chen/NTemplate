using Microsoft.CSharp;
using System;
using System.CodeDom.Compiler;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Razor;
using System.Web.Razor.Generator;

namespace NTemplate
{
    public class RazorTemplateEngine : ITemplateEngine
    {
        private const string NAMESPACE = "_NTemplate";

        public bool EnableDebug { get; set; }
        public TextWriter DebugOutput { get; set; }

        public string Execute(string template, dynamic Model)
        {
            string defaultNamespace = NAMESPACE;
            string defaultClassName = GetClassName();
            GeneratorResults generatorResults = Parse(template, defaultNamespace, defaultClassName);
            return ExecuteInternal(generatorResults, defaultNamespace, defaultClassName, Model);
        }

        private string ExecuteInternal(GeneratorResults razorResults, string defaultNamespace, string defaultClassname, dynamic Model)
        {
            using (var provider = new CSharpCodeProvider())
            {
                var compiler = new CompilerParameters();
                compiler.ReferencedAssemblies.Add("System.dll");
                compiler.ReferencedAssemblies.Add("System.Core.dll");
                compiler.ReferencedAssemblies.Add("Microsoft.CSharp.dll");
                compiler.ReferencedAssemblies.Add(Assembly.GetExecutingAssembly().Location);
                compiler.GenerateInMemory = true;
                var result = provider.CompileAssemblyFromDom(compiler, razorResults.GeneratedCode);
                if (result.Errors.HasErrors)
                {
                    var error = result.Errors.OfType<CompilerError>().Where(i => !i.IsWarning).FirstOrDefault();
                    if (error != null) throw new Exception(error.ErrorText); 
                }
                TemplateBase template = (TemplateBase)result.CompiledAssembly.CreateInstance(defaultNamespace + "." + defaultClassname);
                template.Model = Model;
                template.Execute();
                return template.Output.ToString();
            }
        }

        private GeneratorResults Parse(string template, string defaultNamespace, string defaultClassName)
        {
            var host = new RazorEngineHost(new CSharpRazorCodeLanguage());
            host.DefaultBaseClass = typeof(TemplateBase).FullName;
            host.DefaultNamespace = defaultNamespace;
            host.DefaultClassName = defaultClassName;
            host.NamespaceImports.Add("System");
            host.GeneratedClassContext = new GeneratedClassContext("Execute", "Write", "WriteLiteral");

            GeneratorResults generatorResults;
            var engine = new System.Web.Razor.RazorTemplateEngine(host);
            using (var reader = new StringReader(template))
            {
                generatorResults = engine.GenerateCode(reader);

                if(EnableDebug && DebugOutput != null)
                {
                    GenerateDebugInfo(generatorResults);
                }
            }
            return generatorResults;
        }

        private string GetClassName()
            => "_" + Guid.NewGuid().ToString("N");

        private void GenerateDebugInfo(GeneratorResults generatorResults)
        {
            using (DebugOutput)
            {
                CSharpCodeProvider codeProvider = new CSharpCodeProvider();
                CodeGeneratorOptions options = new CodeGeneratorOptions();
                options.BracingStyle = "C";

                using (IndentedTextWriter indentwriter = new IndentedTextWriter(DebugOutput, "    "))
                {
                    codeProvider.GenerateCodeFromCompileUnit(generatorResults.GeneratedCode, indentwriter, options);
                    indentwriter.Flush();
                    indentwriter.Close();
                }
            }
        }
    }
}
