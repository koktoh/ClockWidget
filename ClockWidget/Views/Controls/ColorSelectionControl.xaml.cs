using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Media;
using System.Windows.Threading;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Prism.Commands;

namespace ClockWidget.Views.Controls
{
    /// <summary>
    /// ColorSelectionControl.xaml の相互作用ロジック
    /// </summary>
    public partial class ColorSelectionControl : UserControl
    {
        public static readonly DependencyProperty DefaultColorProperty =
            DependencyProperty.Register(nameof(DefaultColor), typeof(Color), typeof(ColorSelectionControl), new PropertyMetadata(Colors.White));

        public static readonly DependencyProperty SelectedColorProperty =
            DependencyProperty.Register(nameof(SelectedColor), typeof(Color), typeof(ColorSelectionControl), new FrameworkPropertyMetadata(Colors.White, FrameworkPropertyMetadataOptions.BindsTwoWayByDefault, OnSelectedColorChanged));

        private static void OnSelectedColorChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var control = d as ColorSelectionControl;

            if (e.NewValue is Color newColor)
            {
                control.ColorText.Content = newColor;
                control.ColorPreview.Background = new SolidColorBrush(newColor);
                control.UpdateSelectingColor(newColor);
            }
        }

        private readonly ILogger _logger;

        private bool _isSubmitted = false;

        private Color _selectingColor = Colors.White;

        public Color DefaultColor
        {
            get { return (Color)this.GetValue(DefaultColorProperty); }
            set
            {
                this.ColorSelector.DefaultColor = value;
                this.SetValue(DefaultColorProperty, value);
            }
        }

        public Color SelectedColor
        {
            get { return (Color)this.GetValue(SelectedColorProperty); }
            set { this.SetValue(SelectedColorProperty, value); }
        }

        public ColorSelectionControl()
        {
            InitializeComponent();

            if (DesignerProperties.GetIsInDesignMode(this))
            {
                // デザイナーではNullLoggerを使用
                this._logger = new NullLogger<ColorSelectionControl>();
            }
            else
            {
                // 実行時にはLoggerFactoryからロガーを取得
                this._logger = App.LoggerFactory.CreateLogger<ColorSelectionControl>();
            }

            this.SubmitButtonControl.DefaultButtonClick = new DelegateCommand(this.DefaultButtonClick);
            this.SubmitButtonControl.OkButtonClick = new DelegateCommand(this.OkButtonClick);
            this.SubmitButtonControl.CancelButtonClick = new DelegateCommand(this.CancelButtonClick);

            this.ColorSelector.SelectedColorChanged += this.ColorSelector_SelectedColorChanged;

            this.ColorSelectionPopup.Placement = PlacementMode.Custom;
            this.ColorSelectionPopup.CustomPopupPlacementCallback = PopupPlacementManager.PlacementBottomLeft;

            this.NotifyPopup.Placement = PlacementMode.Custom;
            this.NotifyPopup.CustomPopupPlacementCallback = PopupPlacementManager.PlacementBottomLeft;
        }

        private void UpdateSelectingColor(Color color)
        {
            this._selectingColor = color;
            this.ColorSelector.SelectedColor = color;
        }

        private void DefaultButtonClick()
        {
            this.UpdateSelectingColor(this.DefaultColor);
        }

        private void OkButtonClick()
        {
            this._isSubmitted = true;
            this.ColorSelectionPopupClose();
        }

        private void CancelButtonClick()
        {
            this.ColorSelectionPopupClose();
        }

        private void ColorSelectionPopupOpen()
        {
            this._isSubmitted = false;
            this.ColorSelectionPopup.IsOpen = true;
        }

        private void ColorSelectionPopupClose()
        {
            if (this._isSubmitted)
            {
                this.SelectedColor = this._selectingColor;
            }
            else
            {
                this.UpdateSelectingColor(this.SelectedColor);
            }

            this.ColorSelectionPopup.IsOpen = false;
        }

        private void ShowNotifyPopup(string message, NotifyState state)
        {
            var timer = new DispatcherTimer()
            {
                Interval = TimeSpan.FromSeconds(1.5),
            };

            timer.Tick += (s, e) =>
            {
                this.NotifyPopup.IsOpen = false;
                timer.Stop();
            };

            this.NotifyMessage.Message = message;
            this.NotifyMessage.State = state;
            this.NotifyPopup.IsOpen = true;
            timer.Start();
        }

        private void ColorSelectionControl_Loaded(object sender, RoutedEventArgs e)
        {
            this.ColorText.Content = this.SelectedColor;
            this.ColorPreview.Background = new SolidColorBrush(this.SelectedColor);

            this.ColorSelector.SelectedColor = this.SelectedColor;
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                Clipboard.SetText(this.SelectedColor.ToString());
                this.ShowNotifyPopup("クリップボードにコピーしました", NotifyState.Success);
                this._logger.LogInformation("クリップボードにコピー: {Color}", this.SelectedColor.ToString());
            }
            catch (Exception ex)
            {
                this.ShowNotifyPopup($"コピーに失敗しました", NotifyState.Error);
                this._logger.LogError(ex, "クリップボードへのコピー失敗: {Color}", this.SelectedColor.ToString());
            }
        }

        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            this.ColorSelectionPopupOpen();
        }

        private void ColorSelectionPopup_Closed(object sender, EventArgs e)
        {
            this.ColorSelectionPopupClose();
        }

        private void ColorSelector_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            this._selectingColor = e.NewValue ?? Colors.White;
        }
    }
}
