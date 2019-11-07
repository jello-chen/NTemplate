using System.CodeDom;
using System.Collections.Generic;

namespace NTemplate.Engine.Aspx.Generator
{
    class DirectiveGenerator : StatementGenerator
    {
        public DirectiveGenerator(string directive, Dictionary<string, string> attributes)
        {
            Directive = directive;
            Attributes = attributes;
        }

        public string Directive { get; }
        public Dictionary<string, string> Attributes { get; }

        public override CodeStatement GenerateStatement()
        {
            return null;
        }
    }
}
