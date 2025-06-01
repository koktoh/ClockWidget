using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace ClockWidget.Views.Controls
{
    /// <summary>
    /// NumericUpDown.xaml の相互作用ロジック
    /// </summary>
    public partial class NumericUpDown : UserControl
    {
        public static readonly DependencyProperty ValueProperty =
            DependencyProperty.Register(nameof(Value), typeof(double), typeof(NumericUpDown), new FrameworkPropertyMetadata(0.0, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnValueChanged, CoerceValue));

        private static void OnValueChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
        }

        private static object CoerceValue(DependencyObject d, object baseValue)
        {
            var control = (NumericUpDown)d;
            var value = (double)baseValue;

            if (value < control.Min) return control.Min;
            if (value > control.Max) return control.Max;

            return baseValue;
        }

        public static readonly DependencyProperty MinProperty =
            DependencyProperty.Register(nameof(Min), typeof(double), typeof(NumericUpDown), new PropertyMetadata(0.0));

        public static readonly DependencyProperty MaxProperty =
            DependencyProperty.Register(nameof(Max), typeof(double), typeof(NumericUpDown), new PropertyMetadata(100.0));

        public static readonly DependencyProperty StepProperty =
            DependencyProperty.Register(nameof(Step), typeof(double), typeof(NumericUpDown), new PropertyMetadata(1.0));

        public double Value
        {
            get { return (double)GetValue(ValueProperty); }
            set { SetValue(ValueProperty, value); }
        }

        public double Min
        {
            get { return (double)GetValue(MinProperty); }
            set { SetValue(MinProperty, value); }
        }

        public double Max
        {
            get { return (double)GetValue(MaxProperty); }
            set { SetValue(MaxProperty, value); }
        }

        public double Step
        {
            get { return (double)GetValue(StepProperty); }
            set { SetValue(StepProperty, value); }
        }

        public NumericUpDown()
        {
            InitializeComponent();

            this.NotifyPopup.Placement = PlacementMode.Custom;
            this.NotifyPopup.CustomPopupPlacementCallback = PopupPlacementManager.PlacementBottomLeft;
        }

        private void Increment(double step)
        {
            this.Value += step;
        }

        private void Decrement(double step)
        {
            this.Value -= step;
        }

        private void CommitInputValueIfValid()
        {
            if (double.TryParse(this.InputBox.Text, out double parsed))
            {
                this.Value = parsed;
            }
        }

        private bool IsTextAllowed(string text)
        {
            return double.TryParse(text, out _);
        }

        private void InputBox_PreviewKeyDown(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                this.CommitInputValueIfValid();
                e.Handled = true;
            }
            else if (e.Key == Key.Up)
            {
                this.CommitInputValueIfValid();
                this.Increment(this.Step);
                e.Handled = true;
            }
            else if (e.Key == Key.Down)
            {
                this.CommitInputValueIfValid();
                this.Decrement(this.Step);
                e.Handled = true;
            }
            else if (e.Key == Key.PageUp)
            {
                this.CommitInputValueIfValid();
                this.Increment(this.Step * 10);
                e.Handled = true;
            }
            else if (e.Key == Key.PageDown)
            {
                this.CommitInputValueIfValid();
                this.Decrement(this.Step * 10);
                e.Handled = true;
            }
        }

        private void InputBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !this.IsTextAllowed(e.Text);
        }

        private void InputBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (double.TryParse(this.InputBox.Text, out double newValue))
            {
                if (newValue < this.Min || this.Max < newValue)
                {
                    this.NotifyMessage.Message = $"{this.Min} から {this.Max} の範囲で指定してください";
                    this.NotifyPopup.IsOpen = true;
                }
                else
                {
                    this.NotifyPopup.IsOpen = false;
                }
            }
        }

        private void UpButton_Click(object sender, RoutedEventArgs e)
        {
            this.Increment(this.Step);
        }

        private void DownButton_Click(object sender, RoutedEventArgs e)
        {
            this.Decrement(this.Step);
        }
    }
}
