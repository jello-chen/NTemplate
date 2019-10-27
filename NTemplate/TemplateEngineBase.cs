using System;
using System.IO;

namespace NTemplate
{
    public abstract class TemplateEngineBase : ITemplateEngine
    {
        public virtual bool EnableDebug { get; set; }
        public virtual TextWriter DebugOutput { get; set; } = Console.Out;

        public abstract string Render(string template, dynamic Model);
    }
}
