using System;
using System.Windows;
using System.Windows.Data;

namespace ClockWidget.Views.Controls.Converters
{
    internal class MultiplyConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            try
            {
                var dValue = System.Convert.ToDouble(value, culture);
                var multiplier = System.Convert.ToDouble(parameter, culture);

                return dValue * multiplier;
            }
            catch
            {
                return DependencyProperty.UnsetValue;
            }
        }
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
