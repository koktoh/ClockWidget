namespace ClockWidget.Models.Weather.Amedas
{
    public record AmedasLocation
    {
        public string LocationId { get; init; }
        public string Type { get; init; }
        public string Elems { get; init; }
        public double Latitude { get; init; }
        public double Longitude { get; init; }
        public int Alt { get; init; }
        public string KanjiName { get; init; }
        public string KanaName { get; init; }
        public string EnName { get; init; }
    }
}
