using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ClockWidget.Views.Controls.Converters
{
    [ValueConversion(typeof(NotifyState), typeof(Brush))]
    internal class NotifyStateToBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is NotifyState state ? NotifyVisuals.GetBackgroundBrush(state) : Brushes.Transparent;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
