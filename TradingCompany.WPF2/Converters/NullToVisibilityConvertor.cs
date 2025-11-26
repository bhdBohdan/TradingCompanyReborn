using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TradingCompany.WPF2.Converters
{
    // ConverterParameter = "Invert" will invert the result (null -> Visible).
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool isNull = value == null;
            bool invert = (parameter as string)?.Equals("Invert", StringComparison.OrdinalIgnoreCase) == true;
            bool visible = invert ? isNull : !isNull;
            return visible ? Visibility.Visible : Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
            => throw new NotSupportedException();
    }
}