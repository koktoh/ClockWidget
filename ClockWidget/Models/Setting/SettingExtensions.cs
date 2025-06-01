using ClockWidget.Common;

namespace ClockWidget.Models.Setting
{
    public static class SettingExtensions
    {
        public static Setting Clone(this IReadonlySetting setting)
        {
            return DeepCopyHelper.DeepCopy((Setting)setting);
        }
    }
}
