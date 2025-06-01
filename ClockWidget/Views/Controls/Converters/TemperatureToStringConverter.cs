using System;
using System.Globalization;
using System.Windows.Data;

namespace ClockWidget.Views.Controls.Converters
{
    [ValueConversion(typeof(double), typeof(string))]
    internal class TemperatureToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is double temperature)
            {
                return $"{temperature:0.0}";
            }

            return "--";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
