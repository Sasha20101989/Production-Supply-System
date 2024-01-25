using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows;

namespace UI_Interface.Converters
{
    /// <summary>
    /// Конвертер для инвертирования логического значения.
    /// </summary>
    public class InverseBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Преобразует логическое значение в его инвертированное значение.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue ? !boolValue : DependencyProperty.UnsetValue;
        }

        /// <summary>
        /// Не реализовано, так как инвертирование назад не поддерживается.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}