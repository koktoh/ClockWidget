using ClockWidget.Models.Service;

namespace ClockWidget.Models.Net
{
    public interface INetworkAccessPolicyService : IClockWidgetService
    {
        bool IsAllowed();
        bool IsAllowedService<TService>(TService service) where TService : IClockWidgetService;
        bool IsAllowedByKey(string key);
    }
}
