using System.IO;

namespace NTemplate.Engine.Razor
{
    public class RazorTemplateEngine : TemplateEngineBase
    {
        public RazorTemplateEngine() : this(null)
        {

        }

        public RazorTemplateEngine(TextWriter debugOutput) : base(new TemplateCompiler(new RazorTemplateParser(), debugOutput))
        {

        }
    }
}
