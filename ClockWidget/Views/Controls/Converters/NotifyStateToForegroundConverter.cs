using System;
using System.Globalization;
using System.Windows.Data;
using System.Windows.Media;

namespace ClockWidget.Views.Controls.Converters
{
    [ValueConversion(typeof(NotifyState), typeof(Brush))]
    internal class NotifyStateToForegroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is NotifyState state ? NotifyVisuals.GetForegroundBrush(state) : Brushes.Black;
        }
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
