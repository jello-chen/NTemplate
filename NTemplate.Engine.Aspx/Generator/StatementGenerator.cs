using System.CodeDom;

namespace NTemplate.Engine.Aspx.Generator
{
    abstract class StatementGenerator
    {
        public abstract CodeStatement GenerateStatement();
    }
}
