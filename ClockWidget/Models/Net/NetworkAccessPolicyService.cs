using System;
using System.Collections.Generic;
using ClockWidget.Models.Service;
using ClockWidget.Models.Setting;
using ClockWidget.Models.Weather;

namespace ClockWidget.Models.Net
{
    internal class NetworkAccessPolicyService : INetworkAccessPolicyService
    {
        private readonly IReadOnlyDictionary<string, Func<IReadonlySetting, bool>> _policyMap = new Dictionary<string, Func<IReadonlySetting, bool>>
        {
            { nameof(IWeatherService), setting => setting.ShowWeather },
        };

        private readonly ISettingService _settingService;
        private readonly INetworkAccessibilityService _networkAccessibilityService;

        public NetworkAccessPolicyService(ISettingService settingService, INetworkAccessibilityService networkAccessibilityService)
        {
            this._settingService = settingService;
            this._networkAccessibilityService = networkAccessibilityService;
        }

        public bool IsAllowed()
        {
            return !this._settingService.Current.StandAlone && this._networkAccessibilityService.IsAccessible;
        }

        public bool IsAllowedService<TService>(TService service)
            where TService : IClockWidgetService
        {
            return this.IsAllowedByKey(this.GetInterfaceName(service));
        }

        public bool IsAllowedByKey(string key)
        {
            return this._policyMap.TryGetValue(key, out var policy) && policy(this._settingService.Current);
        }

        /// <summary>
        /// ClockWidget のサービスインターフェイス名を取得する。
        /// <seealso cref="IClockWidgetService"/> を継承するサービスインターフェイスを対象とする。
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <param name="service"></param>
        /// <returns>サービスインターフェース名</returns>
        private string GetInterfaceName<TService>(TService service)
            where TService : IClockWidgetService
        {
            var type = service.GetType();

            foreach (var iface in type.GetInterfaces())
            {
                if (iface is IClockWidgetService) continue;

                if (iface.IsAssignableTo(typeof(IClockWidgetService)))
                {
                    return iface.Name;
                }
            }
            return string.Empty;
        }
    }
}
