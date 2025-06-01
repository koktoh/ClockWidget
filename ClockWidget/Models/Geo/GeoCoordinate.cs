using System.Text.Json.Serialization;

namespace ClockWidget.Models.Geo
{
    public record GeoCoordinate
    {
        /// <summary>
        /// 緯度
        /// </summary>
        [JsonPropertyName("lat")]
        public double Latitude { get; init; }
        /// <summary>
        /// 経度
        /// </summary>
        [JsonPropertyName("lon")]
        public double Longitude { get; init; }

        public GeoCoordinate(double latitude, double longitude)
        {
            this.Latitude = latitude;
            this.Longitude = longitude;
        }
    }
}
