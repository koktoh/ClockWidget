using System;
using System.Globalization;
using System.Windows.Data;

namespace ClockWidget.Views.Controls.Converters
{
    [ValueConversion(typeof(bool), typeof(bool))]
    internal class BooleanNagationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return !booleanValue;
            }
            return false;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return !booleanValue;
            }
            return false;
        }
    }
}
