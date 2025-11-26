using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace TradingCompany.WPF2.Converters
{
    public class BooleanToVisibilityConverter : IValueConverter
    {
        // Convert bool to Visibility
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            return Visibility.Collapsed; // Or Visibility.Hidden
        }

        // Convert back (usually not needed for this scenario)
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
                return visibility == Visibility.Visible;
            return false;
        }
    }
}