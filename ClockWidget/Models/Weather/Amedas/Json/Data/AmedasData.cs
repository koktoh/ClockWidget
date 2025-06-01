using System.Text.Json.Serialization;
using ClockWidget.Models.Weather.Amedas.Json.Converters;

namespace ClockWidget.Models.Weather.Amedas.Json.Data
{
    public record AmedasData
    {
        [JsonPropertyName("pressure")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Pressure { get; init; }
        [JsonPropertyName("normalPressure")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> NormalPressure { get; init; }
        [JsonPropertyName("temp")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Temperature { get; init; }
        [JsonPropertyName("humidity")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Humidity { get; init; }
        [JsonPropertyName("visibility")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Visiblity { get; init; }
        [JsonPropertyName("snow")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Snow { get; init; }
        [JsonPropertyName("weather")]
        [JsonConverter(typeof(JsonAmedasWeatherCodeDataElementConverter))]
        public AmedasDataElement<WeatherCode> Weather { get; init; }
        [JsonPropertyName("snow1h")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Snow1h { get; init; }
        [JsonPropertyName("snow6h")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Snow6h { get; init; }
        [JsonPropertyName("snow12h")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Snow12h { get; init; }
        [JsonPropertyName("snow24h")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Snow24h { get; init; }
        [JsonPropertyName("sun10m")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Sun10m { get; init; }
        [JsonPropertyName("sun1h")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Sun1h { get; init; }
        [JsonPropertyName("precipitation10m")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Precipitation10m { get; init; }
        [JsonPropertyName("precipitation1h")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Precipitation1h { get; init; }
        [JsonPropertyName("precipitation3h")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Precipitation3h { get; init; }
        [JsonPropertyName("precipitation24h")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Precipitation24h { get; init; }
        [JsonPropertyName("windDirection")]
        [JsonConverter(typeof(JsonAmedasWindDirectionDataElementConverter))]
        public AmedasDataElement<WindDirection> WindDirection { get; init; }
        [JsonPropertyName("wind")]
        [JsonConverter(typeof(JsonAmedasDoubleDataElementConverter))]
        public AmedasDataElement<double> Wind { get; init; }
    }
}
