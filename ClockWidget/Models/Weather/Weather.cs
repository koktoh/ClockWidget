namespace ClockWidget.Models.Weather
{
    public record Weather
    {
        public double? Temperature { get; init; } = null;
        public WeatherCode WeatherCode { get; init; } = WeatherCode.Unknown;
    }
}
