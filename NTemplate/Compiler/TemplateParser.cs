using System;

namespace NTemplate
{
    public abstract class TemplateParser : ITemplateParser
    {
        protected virtual string GetCompiledTemplateNamespace() => TemplateCompilerSettings.Namespace;
        protected virtual string GetCompiledTemplateClass() => "_" + Guid.NewGuid().ToString("N");
        protected virtual string GetCompiledTemplateMethodName() => TemplateCompilerSettings.MethodName;

        public abstract TemplateParseResult ParseTemplate(string template);
    }
}
