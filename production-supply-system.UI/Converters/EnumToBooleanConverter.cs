using System;
using System.Globalization;
using System.Windows.Data;

namespace UI_Interface.Converters
{
    /// <summary>
    /// Конвертер для преобразования значения перечисления в логическое значение и обратно.
    /// </summary>
    public class EnumToBooleanConverter : IValueConverter
    {
        /// <summary>
        /// Тип перечисления.
        /// </summary>
        public Type EnumType { get; set; }

        /// <summary>
        /// Преобразует значение перечисления в логическое значение.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (parameter is string enumString)
            {
                if (Enum.IsDefined(EnumType, value))
                {
                    object enumValue = Enum.Parse(EnumType, enumString);

                    return enumValue.Equals(value);
                }
            }

            return false;
        }

        /// <summary>
        /// Преобразует логическое значение обратно в значение перечисления.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return parameter is string enumString ? Enum.Parse(EnumType, enumString) : null;
        }
    }
}
