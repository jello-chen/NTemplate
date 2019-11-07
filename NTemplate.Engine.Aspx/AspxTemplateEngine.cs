using NTemplate.Engine.Aspx.Parser.SyntaxTree;
using System.IO;

namespace NTemplate.Engine.Aspx
{
    public class AspxTemplateEngine : TemplateEngineBase
    {
        public AspxTemplateEngine() : this(null)
        {

        }
        public AspxTemplateEngine(TextWriter debugOutput) : base(new TemplateCompiler(new AspxTemplateParser(), debugOutput))
        {

        }
    }
}
