using System.Windows;
using System.Windows.Controls;

namespace ClockWidget.Views.Controls
{
    public class FieldRow : ContentControl
    {
        static FieldRow()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(FieldRow), new FrameworkPropertyMetadata(typeof(FieldRow)));
        }

        public static readonly DependencyProperty LabelProperty =
            DependencyProperty.Register(nameof(Label), typeof(string), typeof(FieldRow), new PropertyMetadata(string.Empty));

        public string Label
        {
            get { return (string)GetValue(LabelProperty); }
            set { SetValue(LabelProperty, value); }
        }
    }
}
