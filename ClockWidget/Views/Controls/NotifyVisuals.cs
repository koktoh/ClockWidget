using System.Windows;
using System.Windows.Media;
using MaterialDesignThemes.Wpf;

namespace ClockWidget.Views.Controls
{
    public static class NotifyVisuals
    {
        public static Brush GetForegroundBrush(NotifyState state)
        {
            return TryGetBrush($"InlineNotify.{state}Foreground") ?? Brushes.Black;
        }

        public static Brush GetBackgroundBrush(NotifyState state)
        {
            return TryGetBrush($"InlineNotify.{state}Background") ?? Brushes.Transparent;
        }

        public static PackIconKind GetIcon(NotifyState state)
        {
            return state switch
            {
                NotifyState.Info => PackIconKind.Info,
                NotifyState.Success => PackIconKind.CheckCircle,
                NotifyState.Warning => PackIconKind.Alert,
                NotifyState.Error => PackIconKind.Error,
                _ => PackIconKind.Info,
            };
        }

        private static Brush TryGetBrush(string key)
        {
            return Application.Current?.TryFindResource(key) as Brush;
        }
    }
}
