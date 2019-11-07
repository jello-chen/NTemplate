using System.CodeDom;

namespace NTemplate
{
    public class TemplateParseResult
    {
        public CodeCompileUnit CodeCompileUnit { get; set; }
        public string Namespace { get; set; }
        public string Class { get; set; }
    }
}
