using System.Reflection;

namespace NTemplate
{
    public class TemplateCompileResult
    {
        public Assembly CompiledAssembly { get; set; }
        public string Namespace { get; set; }
        public string Class { get; set; }
    }
}
