namespace NTemplate
{
    public interface ITemplateEngine
    {
        string Execute(string template, dynamic Model);
    }
}
