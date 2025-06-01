using System.Threading.Tasks;
using ClockWidget.Models.Service;

namespace ClockWidget.Models.Weather
{
    public interface IWeatherService : IClockWidgetService
    {
        Task<Weather> GetWeatherDataAsync();
    }
}
