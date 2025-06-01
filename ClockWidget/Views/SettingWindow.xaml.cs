using ClockWidget.ViewModels;
using MahApps.Metro.Controls;

namespace ClockWidget.Views
{
    /// <summary>
    /// SettingsWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class SettingWindow : MetroWindow
    {
        public SettingWindow()
        {
            InitializeComponent();

            this.Loaded += (s, e) =>
            {
                if (this.DataContext is SettingWindowViewModel vm)
                {
                    vm.RequestClose += () =>
                    {
                        this.Close();
                    };
                }
            };
        }
    }
}
