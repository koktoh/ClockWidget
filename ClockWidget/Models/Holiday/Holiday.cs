using System;

namespace ClockWidget.Models.Holiday
{
    public record Holiday
    {
        public DateTime Date { get; init; }
        public string Name { get; init; }
    }
}
