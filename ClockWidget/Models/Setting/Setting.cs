using System.Linq;
using System.Text.Json.Serialization;
using System.Windows.Media;

namespace ClockWidget.Models.Setting
{
    public class Setting : IReadonlySetting
    {
        private const byte DEFAULT_RED = 0xEE;
        private const byte DEFAULT_GREEN = 0xEE;
        private const byte DEFAULT_BLUE = 0xEE;

        public int Size { get; set; } = 500;
        public double WindowTop { get; set; } = 0;
        public double WindowLeft { get; set; } = 0;
        public bool StandAlone { get; set; } = false;
        public bool Topmost { get; set; } = true;
        public bool ShowDate { get; set; } = true;
        public bool ShowWeather { get; set; } = true;
        [JsonConverter(typeof(JsonFontFamilyConverter))]
        public FontFamily MainFontFamily { get; set; } = new FontFamily("Impact");
        [JsonConverter(typeof(JsonFontFamilyConverter))]
        public FontFamily HolidayFontFamily { get; set; } = new FontFamily("Yu Gothic UI Semibold");
        [JsonConverter(typeof(JsonColorConverter))]
        public Color ClockFaceColor { get; set; } = Color.FromRgb(DEFAULT_RED, DEFAULT_GREEN, DEFAULT_BLUE);
        [JsonConverter(typeof(JsonColorConverter))]
        public Color HourHandColor { get; set; } = Colors.OrangeRed;
        [JsonConverter(typeof(JsonColorConverter))]
        public Color MinuteHandColor { get; set; } = Colors.OrangeRed;
        [JsonConverter(typeof(JsonColorConverter))]
        public Color SecondHandColor { get; set; } = Color.FromRgb(DEFAULT_RED, DEFAULT_GREEN, DEFAULT_BLUE);
        [JsonConverter(typeof(JsonColorConverter))]
        public Color DateColor { get; set; } = Color.FromRgb(DEFAULT_RED, DEFAULT_GREEN, DEFAULT_BLUE);

        public override bool Equals(object obj)
        {
            if (obj is not Setting setting) return false;

            return this.GetType()
                .GetProperties()
                .Where(p => p.CanRead && p.CanWrite)
                .All(p =>
                {
                    var source = p.GetValue(this);
                    var target = p.GetValue(setting);

                    if (source is null && target is null) return true;

                    if ((source is null && target is not null) || (source is not null && target is null)) return false;

                    return source.Equals(target);
                });
        }

        public override int GetHashCode()
        {
            return this.GetType()
                .GetProperties()
                .Where(p => p.CanRead && p.CanWrite)
                .Select(p => p.GetValue(this))
                .Aggregate(0, (current, value) => current ^ (value?.GetHashCode() ?? 0));
        }
    }
}
