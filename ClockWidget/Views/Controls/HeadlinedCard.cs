using System.Windows;
using System.Windows.Controls;

namespace ClockWidget.Views.Controls
{
    public class HeadlinedCard : ContentControl
    {
        static HeadlinedCard()
        {
            DefaultStyleKeyProperty.OverrideMetadata(typeof(HeadlinedCard), new FrameworkPropertyMetadata(typeof(HeadlinedCard)));
        }

        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register("Header", typeof(string), typeof(HeadlinedCard), new PropertyMetadata(string.Empty));

        public string Header
        {
            get { return (string)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }
    }
}
