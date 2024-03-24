using System.Globalization;
using System.Windows;

using production_supply_system.EntityFramework.DAL.Extensions;


namespace UI_Interface.Multilang
{
    public class MultilangManager : IMultilangManager
    {
        public Languages GetCurrentLanguage()
        {
            if (Application.Current.Properties.Contains("Language"))
            {
                string language = Application.Current.Properties["Language"].ToString();

                if (language == EnumExtensions.GetDescription(Languages.ru))
                {
                    return Languages.ru;
                }
                else if (language == EnumExtensions.GetDescription(Languages.en))
                {
                    return Languages.en;
                }

                return Languages.ru;
            }

            return Languages.ru;
        }

        public void InitializeLanguage()
        {
            SetLanguage(GetCurrentLanguage());
        }

        public void SetLanguage(Languages language)
        {
            _ = new TemporaryThreadCulture(new CultureInfo(EnumExtensions.GetDescription(language)));
            Application.Current.Properties["Language"] = EnumExtensions.GetDescription(language);
        }
    }
}
