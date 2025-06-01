using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace ClockWidget.Views.Controls
{
    /// <summary>
    /// SubmitButtonControl.xaml の相互作用ロジック
    /// </summary>
    public partial class SubmitButtonControl : UserControl
    {
        public static readonly DependencyProperty DefaultButtonClickProperty =
            DependencyProperty.Register(nameof(DefaultButtonClick), typeof(ICommand), typeof(SubmitButtonControl), new PropertyMetadata(null));

        public static readonly DependencyProperty OkButtonClickProperty =
            DependencyProperty.Register(nameof(OkButtonClick), typeof(ICommand), typeof(SubmitButtonControl), new PropertyMetadata(null));

        public static readonly DependencyProperty CancelButtonClickProperty =
            DependencyProperty.Register(nameof(CancelButtonClick), typeof(ICommand), typeof(SubmitButtonControl), new PropertyMetadata(null));

        public ICommand DefaultButtonClick
        {
            get { return (ICommand)this.GetValue(DefaultButtonClickProperty); }
            set { this.SetValue(DefaultButtonClickProperty, value); }
        }
        public ICommand OkButtonClick
        {
            get { return (ICommand)this.GetValue(OkButtonClickProperty); }
            set { this.SetValue(OkButtonClickProperty, value); }
        }
        public ICommand CancelButtonClick
        {
            get { return (ICommand)this.GetValue(CancelButtonClickProperty); }
            set { this.SetValue(CancelButtonClickProperty, value); }
        }

        public SubmitButtonControl()
        {
            InitializeComponent();
        }

        private void DefaultButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.DefaultButtonClick?.CanExecute(null) == true)
            {
                this.DefaultButtonClick.Execute(null);
            }
        }

        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.OkButtonClick?.CanExecute(null) == true)
            {
                this.OkButtonClick.Execute(null);
            }
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            if (this.CancelButtonClick?.CanExecute(null) == true)
            {
                this.CancelButtonClick.Execute(null);
            }
        }
    }
}
