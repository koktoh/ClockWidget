using ClockWidget.Models.Setting;
using Prism.Events;

namespace ClockWidget.Events
{
    internal class SettingChangedEvent : PubSubEvent<Setting>
    {
    }
}
