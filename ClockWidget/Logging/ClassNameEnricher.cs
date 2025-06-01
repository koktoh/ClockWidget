using System.Linq;
using Serilog.Core;
using Serilog.Events;

namespace ClockWidget.Logging
{
    public class ClassNameEnricher : ILogEventEnricher
    {
        public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
        {
            var className = logEvent.Properties.TryGetValue("SourceContext", out var sourceContext) ? sourceContext.ToString().Trim('"').Split('.').Last() : string.Empty;

            logEvent.AddOrUpdateProperty(propertyFactory.CreateProperty("ClassName", className));
        }
    }
}
