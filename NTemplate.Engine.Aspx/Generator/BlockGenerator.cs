using System.CodeDom;

namespace NTemplate.Engine.Aspx.Generator
{
    class BlockGenerator : StatementGenerator
    {
        public string Text;

        public BlockGenerator(string text) => Text = text;

        public override CodeStatement GenerateStatement()
        {
            return new CodeSnippetStatement(Text);
        }
    }
}
