using System.CodeDom;

namespace NTemplate.Engine.Aspx.Generator
{
    class CommentGenerator : StatementGenerator
    {
        public CommentGenerator(string comment)
        {
            Comment = comment;
        }

        public string Comment { get; }

        public override CodeStatement GenerateStatement()
        {
            return null;
        }
    }
}
