namespace UI_Interface.Multilang
{
    public interface IMultilangManager
    {
        void InitializeLanguage();
        void SetLanguage(Languages language);
        Languages GetCurrentLanguage();
    }
}
