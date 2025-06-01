using System.Threading.Tasks;
using ClockWidget.Models.Service;

namespace ClockWidget.Models.Setting
{
    public interface ISettingService : IClockWidgetService
    {
        IReadonlySetting Default { get; }
        IReadonlySetting Current { get; }

        void Load();
        Task LoadAsync();
        void Save(Setting setting);
        Task SaveAsync(Setting setting);
    }
}
