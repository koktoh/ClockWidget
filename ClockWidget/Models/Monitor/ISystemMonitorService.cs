using System;
using ClockWidget.Models.Service;

namespace ClockWidget.Models.Monitor
{
    public interface ISystemMonitorService : IClockWidgetService, IDisposable
    {
        void Initialize();
    }
}
