namespace NTemplate
{
    public abstract class TemplateBase<TModel> : TemplateBase
    {
        public new virtual TModel Model 
        {
            get => (TModel)base.Model;
            set => base.Model = value;
        }

        public virtual string Generate(TModel model)
        {
            Model = model;
            Execute();
            return Output.ToString();
        }
    }
}
