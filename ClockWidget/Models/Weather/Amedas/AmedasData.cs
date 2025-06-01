using ClockWidget.Models.Weather.Amedas.Json.Data;

namespace ClockWidget.Models.Weather.Amedas
{
    public record AmedasData
    {
        public string LocationId { get; init; }
        public AmedasDataElement<double> Pressure { get; init; }
        public AmedasDataElement<double> NormalPressure { get; init; }
        public AmedasDataElement<double> Temperature { get; init; }
        public AmedasDataElement<double> Humidity { get; init; }
        public AmedasDataElement<double> Visiblity { get; init; }
        public AmedasDataElement<double> Snow { get; init; }
        public AmedasDataElement<WeatherCode> Weather { get; init; }
        public AmedasDataElement<double> Snow1h { get; init; }
        public AmedasDataElement<double> Snow6h { get; init; }
        public AmedasDataElement<double> Snow12h { get; init; }
        public AmedasDataElement<double> Snow24h { get; init; }
        public AmedasDataElement<double> Sun10m { get; init; }
        public AmedasDataElement<double> Sun1h { get; init; }
        public AmedasDataElement<double> Precipitation10m { get; init; }
        public AmedasDataElement<double> Precipitation1h { get; init; }
        public AmedasDataElement<double> Precipitation3h { get; init; }
        public AmedasDataElement<double> Precipitation24h { get; init; }
        public AmedasDataElement<WindDirection> WindDirection { get; init; }
        public AmedasDataElement<double> Wind { get; init; }
    }
}
