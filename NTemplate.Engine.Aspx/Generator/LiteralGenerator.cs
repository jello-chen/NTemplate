using System.CodeDom;

namespace NTemplate.Engine.Aspx.Generator
{
    class LiteralGenerator : StatementGenerator
    {
        public LiteralGenerator(string text) => Text = text;

        public string Text { get; }

        public override CodeStatement GenerateStatement()
        {
            CodeExpression textExpression = new CodePrimitiveExpression(Text);
            CodeExpression builderObject = new CodeVariableReferenceExpression("Output");
            CodeExpression appendExpression = new CodeMethodInvokeExpression(builderObject, "Append", textExpression);
            return new CodeExpressionStatement(appendExpression);
        }
    }
}
