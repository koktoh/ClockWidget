using System;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Interop;

namespace ClockWidget.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.SourceInitialized += this.MainWindow_SourceInitialized;
        }

        private void MainWindow_SourceInitialized(object sender, EventArgs e)
        {
            IntPtr hwnd = new WindowInteropHelper(this).Handle;
            int exStyle = GetWindowLong(hwnd, GWL_EXSTYLE);
            exStyle |= WS_EX_TOOLWINDOW;
            SetWindowLong(hwnd, GWL_EXSTYLE, exStyle);
        }

        private void MainGrid_MouseRightButtonDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            var contextMenu = this.FindResource("SharedContextMenu") as ContextMenu;

            var clone = this.CloneContextMenu(contextMenu);

            clone.PlacementTarget = this.MainGrid;
            clone.Placement = System.Windows.Controls.Primitives.PlacementMode.MousePoint;
            clone.IsOpen = true;
            e.Handled = true;
        }

        private ContextMenu CloneContextMenu(ContextMenu original)
        {
            var clone = new ContextMenu();

            foreach (var item in original.Items)
            {
                if (item is MenuItem menuItem)
                {
                    var cloneItem = this.CloneMenuItem(menuItem);
                    clone.Items.Add(cloneItem);
                }
                else if (item is Separator)
                {
                    clone.Items.Add(new Separator());
                }
                else
                {
                    // 他のアイテムタイプはそのまま追加
                    // 必要になったら他のアイテムタイプも処理する
                    clone.Items.Add(item);
                }
            }

            return clone;
        }

        private MenuItem CloneMenuItem(MenuItem original)
        {
            var clone = new MenuItem
            {
                Header = original.Header,
                Command = original.Command,
                CommandParameter = original.CommandParameter,
                Icon = original.Icon,
                IsEnabled = original.IsEnabled,
                IsChecked = original.IsChecked,
                IsCheckable = original.IsCheckable,
            };

            foreach (var item in original.Items)
            {
                if (item is MenuItem menuItem)
                {
                    var cloneItem = this.CloneMenuItem(menuItem);
                    clone.Items.Add(cloneItem);
                }
                else
                {
                    // 他のアイテムタイプはそのまま追加
                    // 必要になったら他のアイテムタイプも処理する
                    clone.Items.Add(item);
                }
            }

            return clone;
        }

        private const int GWL_EXSTYLE = -20;
        private const int WS_EX_TOOLWINDOW = 0x00000080;

        [DllImport("user32.dll", SetLastError = true)]
        static extern int GetWindowLong(IntPtr hWnd, int nIndex);

        [DllImport("user32.dll", SetLastError = true)]
        static extern int SetWindowLong(IntPtr hWnd, int nIndex, int dwNewLong);
    }
}
