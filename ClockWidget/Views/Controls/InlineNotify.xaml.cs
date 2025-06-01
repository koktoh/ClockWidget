using System.Windows;
using System.Windows.Controls;

namespace ClockWidget.Views.Controls
{
    /// <summary>
    /// InlineNotify.xaml の相互作用ロジック
    /// </summary>
    public partial class InlineNotify : UserControl
    {
        public static readonly DependencyProperty StateProperty =
            DependencyProperty.Register(nameof(State), typeof(NotifyState), typeof(InlineNotify), new PropertyMetadata(NotifyState.Info));

        public static readonly DependencyProperty MessageProperty =
            DependencyProperty.Register(nameof(Message), typeof(string), typeof(InlineNotify), new PropertyMetadata(string.Empty));

        public NotifyState State
        {
            get { return (NotifyState)GetValue(StateProperty); }
            set { SetValue(StateProperty, value); }
        }

        public string Message
        {
            get { return (string)GetValue(MessageProperty); }
            set { SetValue(MessageProperty, value); }
        }

        public InlineNotify()
        {
            InitializeComponent();
        }
    }
}
