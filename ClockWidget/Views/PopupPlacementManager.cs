using System.Windows;
using System.Windows.Controls.Primitives;

namespace ClockWidget.Views
{
    public static class PopupPlacementManager
    {
        public static CustomPopupPlacement[] PlacementBottomLeft(Size popupSize, Size targetSize, Point offset)
        {
            return
            [
                new CustomPopupPlacement(new Point(0, targetSize.Height), PopupPrimaryAxis.Horizontal),
            ];
        }
    }
}
