using System.Text.Json.Serialization;
using ClockWidget.Models.Weather.Amedas.Json.Converters;

namespace ClockWidget.Models.Weather.Amedas.Json.Location
{
    public record AmedasLocation
    {
        [JsonPropertyName("type")]
        public string Type { get; init; }
        [JsonPropertyName("elems")]
        public string Elems { get; init; }
        [JsonPropertyName("lat")]
        [JsonConverter(typeof(JsonAmedasDmsCoordinateConverter))]
        public AmedasDmsCoordinate Latitude { get; init; }
        [JsonPropertyName("lon")]
        [JsonConverter(typeof(JsonAmedasDmsCoordinateConverter))]
        public AmedasDmsCoordinate Longitude { get; init; }
        [JsonPropertyName("alt")]
        public int Alt { get; init; }
        [JsonPropertyName("kjName")]
        public string KanjiName { get; init; }
        [JsonPropertyName("knName")]
        public string KanaName { get; init; }
        [JsonPropertyName("enName")]
        public string EnName { get; init; }
    }
}
