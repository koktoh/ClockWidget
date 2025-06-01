namespace ClockWidget.Models.Weather.Amedas.Json.Data
{
    public record AmedasDataElement<T>
    {
        public T Data { get; init; }
        public AQC? AQC { get; init; }
    }
}
