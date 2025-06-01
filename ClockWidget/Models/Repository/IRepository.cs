using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClockWidget.Models.Repository
{
    public interface IRepository<T>
    {
        Task<IEnumerable<T>> GetRecordsAsync();
    }
}
