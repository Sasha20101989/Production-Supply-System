using System.Windows;
using System.Windows.Controls;

using MahApps.Metro.Controls;

namespace UI_Interface.Helpers
{
    /// <summary>
    /// Селектор шаблонов для элементов HamburgerMenu.
    /// </summary>
    public class MenuItemTemplateSelector : DataTemplateSelector
    {
        /// <summary>
        /// Шаблон для элемента с глифом.
        /// </summary>
        public DataTemplate GlyphDataTemplate { get; set; }

        /// <summary>
        /// Шаблон для элемента с изображением.
        /// </summary>
        public DataTemplate ImageDataTemplate { get; set; }

        /// <summary>
        /// Шаблон для элемента с иконкой.
        /// </summary>
        public DataTemplate IconElementTemplate { get; set; }

        /// <summary>
        /// Выбирает шаблон в зависимости от типа элемента HamburgerMenu.
        /// </summary>
        public override DataTemplate SelectTemplate(object item, DependencyObject container)
        {
            return item is HamburgerMenuGlyphItem
                ? GlyphDataTemplate
                : item is HamburgerMenuImageItem
                ? ImageDataTemplate
                : item is HamburgerMenuIconItem ? IconElementTemplate : base.SelectTemplate(item, container);
        }
    }
}
