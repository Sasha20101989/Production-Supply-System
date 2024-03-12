using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace UI_Interface.Converters
{
    public class BooleanToColorConverter : IValueConverter
    {
        public SolidColorBrush TrueColor { get; set; } = Brushes.Black;

        public SolidColorBrush FalseColor { get; set; } = Brushes.IndianRed;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool boolValue ? boolValue ? TrueColor : FalseColor : FalseColor;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
