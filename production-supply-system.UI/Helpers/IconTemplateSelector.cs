using System.Windows.Controls;
using System.Windows;

namespace UI_Interface.Helpers
{
    public class IconTemplateSelector : DataTemplateSelector
    {
        public DataTemplate IconTemplate { get; set; }

        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return IconTemplate;
        }
    }
}
