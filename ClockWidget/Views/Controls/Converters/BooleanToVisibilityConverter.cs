using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ClockWidget.Views.Controls.Converters
{
    [ValueConversion(typeof(bool), typeof(Visibility))]
    internal class BooleanToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is bool booleanValue && booleanValue ? Visibility.Visible : Visibility.Hidden;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
