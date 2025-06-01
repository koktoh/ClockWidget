using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClockWidget.Models.Holiday
{
    public interface IHolidayApiClient
    {
        Task<IEnumerable<Holiday>> GetHolidaysAsync();
    }
}
