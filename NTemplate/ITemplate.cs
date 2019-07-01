namespace NTemplate
{
    public interface ITemplate
    {
        void Write(object value);
        void WriteLiteral(string value);
        void Execute();
    }
}
