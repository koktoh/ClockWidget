using System;
using System.Globalization;
using System.Windows.Data;
using MaterialDesignThemes.Wpf;

namespace ClockWidget.Views.Controls.Converters
{
    [ValueConversion(typeof(NotifyState), typeof(PackIconKind))]
    internal class NotifyStateToIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value is NotifyState state ? NotifyVisuals.GetIcon(state) : PackIconKind.Info;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
