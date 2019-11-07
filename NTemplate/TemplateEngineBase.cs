using NTemplate.Extensions;

namespace NTemplate
{
    public abstract class TemplateEngineBase : ITemplateEngine
    {
        private readonly ITemplateCompiler compiler;

        public TemplateEngineBase(ITemplateCompiler compiler)
        {
            this.compiler = compiler;
        }

        public string Render(string template, dynamic model)
        {
            var compileResult = compiler.CompileTemplate(template);
            return RenderTemplateFromCompileResult(compileResult, model);
        }

        private string RenderTemplateFromCompileResult(TemplateCompileResult compileResult, dynamic model)
        {
            TemplateBase template = (TemplateBase)compileResult.CompiledAssembly.CreateInstance(compileResult.Namespace + "." + compileResult.Class);
            var _model = ((object)model)?.GetType().IsAnonymous() == true ? new DynamicObjectWrapper(model) : model;
            return template.Generate(_model);
        }
    }
}
