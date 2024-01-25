using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows;
using MahApps.Metro.Controls;
using MahApps.Metro.IconPacks;

namespace UI_Interface.Converters
{
    public class BadgeStyleConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool hasError && targetType == typeof(Style))
            {
                Style style = new(typeof(Badged));

                if (hasError)
                {
                    style.Setters.Add(new Setter(Badged.MarginProperty, new Thickness(0, 10, 0, 0)));

                    style.Setters.Add(new Setter(Badged.BadgeBackgroundProperty, Brushes.IndianRed));

                    style.Setters.Add(new Setter(Badged.BadgeProperty, new BootstrapIconsExtension { Kind = PackIconBootstrapIconsKind.FileMinus, Width = 10, Height = 10 }));
                }
                else
                {
                    style.Setters.Add(new Setter(Badged.MarginProperty, new Thickness(0, 10, 0, 0)));

                    style.Setters.Add(new Setter(Badged.BadgeBackgroundProperty, Brushes.Green));

                    style.Setters.Add(new Setter(Badged.BadgeProperty, new BootstrapIconsExtension { Kind = PackIconBootstrapIconsKind.Check2, Width = 10, Height = 10 }));
                }

                return style;
            }

            return null;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
