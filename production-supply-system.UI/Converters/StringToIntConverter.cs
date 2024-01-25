using System;
using System.Globalization;
using System.Windows.Data;

namespace UI_Interface.Converters
{
    public class StringToIntConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is int intValue ? intValue.ToString() : 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return int.TryParse(value as string, out int result) ? result : 0;
        }
    }
}