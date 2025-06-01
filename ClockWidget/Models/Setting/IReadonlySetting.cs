using System.Windows.Media;

namespace ClockWidget.Models.Setting
{
    public interface IReadonlySetting
    {
        int Size { get; }
        double WindowTop { get; }
        double WindowLeft { get; }
        bool StandAlone { get; }
        bool Topmost { get; }
        bool ShowDate { get; }
        bool ShowWeather { get; }
        FontFamily MainFontFamily { get; }
        FontFamily HolidayFontFamily { get; }
        Color ClockFaceColor { get; }
        Color HourHandColor { get; }
        Color MinuteHandColor { get; }
        Color SecondHandColor { get; }
        Color DateColor { get; }
    }
}
