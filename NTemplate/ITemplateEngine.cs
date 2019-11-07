namespace NTemplate
{
    public interface ITemplateEngine
    {
        string Render(string template, dynamic model);
    }
}
