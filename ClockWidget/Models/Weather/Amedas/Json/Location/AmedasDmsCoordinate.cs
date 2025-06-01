namespace ClockWidget.Models.Weather.Amedas.Json.Location
{
    public record AmedasDmsCoordinate
    {
        public int Degree { get; init; }
        public double Minute { get; init; }

        public double ToGeoCoordinate()
        {
            return Degree + Minute / 60;
        }
    }
}
