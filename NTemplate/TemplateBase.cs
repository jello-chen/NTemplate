using System;
using System.IO;

namespace NTemplate
{
    public abstract class TemplateBase : ITemplate, IDisposable
    {
        public TemplateBase() => this.Output = new StringWriter();

        public dynamic Model { get; set; }
        public StringWriter Output { get; private set; }

        public virtual void Write(object value) => this.WriteLiteral(value?.ToString());

        public virtual void WriteLiteral(string value) => this.Output.Write(value);

        public abstract void Execute();

        void IDisposable.Dispose() => this.Output.Dispose();
    }
}
