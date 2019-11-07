using System.CodeDom;

namespace NTemplate.Engine.Aspx.Generator
{
    class ExpressionGenerator : StatementGenerator
    {
        public ExpressionGenerator(string text) => Text = text;

        public string Text { get; }

        public override CodeStatement GenerateStatement()
        {
            CodeExpression textExpression = new CodeSnippetExpression(Text);
            CodeExpression builderObject = new CodeThisReferenceExpression();
            CodeExpression appendExpression = new CodeMethodInvokeExpression(builderObject, "Write", textExpression);
            return new CodeExpressionStatement(appendExpression);
        }
    }
}
