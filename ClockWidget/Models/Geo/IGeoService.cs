using System.Threading.Tasks;
using ClockWidget.Models.Service;

namespace ClockWidget.Models.Geo
{
    public interface IGeoService : IClockWidgetService
    {
        Task<GeoCoordinate> GetGeoDataAsync();
    }
}
