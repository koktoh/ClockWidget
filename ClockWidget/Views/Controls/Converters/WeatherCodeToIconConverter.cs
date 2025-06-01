using System;
using System.Collections.Generic;
using System.Globalization;
using System.Windows.Data;
using ClockWidget.Models.Weather;
using MaterialDesignThemes.Wpf;

namespace ClockWidget.Views.Controls.Converters
{
    [ValueConversion(typeof(WeatherCode), typeof(PackIconKind))]
    internal class WeatherCodeToIconConverter : IValueConverter
    {
        private readonly IReadOnlyDictionary<WeatherCode, PackIconKind> _weatherIconMap = new Dictionary<WeatherCode, PackIconKind>
        {
            { WeatherCode.Clear, PackIconKind.WeatherSunny },
            { WeatherCode.Cloudy, PackIconKind.WeatherCloudy },
            { WeatherCode.Smoke, PackIconKind.WeatherMist },
            { WeatherCode.Fog, PackIconKind.WeatherFog },
            { WeatherCode.PrecipitationOrShowers, PackIconKind.WeatherRainy },
            { WeatherCode.Drizzle, PackIconKind.WeatherDrizzle },
            { WeatherCode.FreezingDrizzle, PackIconKind.WeatherDrizzle },
            { WeatherCode.Rain, PackIconKind.WeatherRainy },
            { WeatherCode.FreezingRain, PackIconKind.WeatherRainy },
            { WeatherCode.Sleet, PackIconKind.WeatherSleet },
            { WeatherCode.Snow, PackIconKind.WeatherSnowy },
            { WeatherCode.IcePellet, PackIconKind.WeatherSnowyRainy },
            { WeatherCode.SnowGrain, PackIconKind.WeatherSnowy },
            { WeatherCode.ShowersOrIntermittentRain, PackIconKind.WeatherPartlyRainy },
            { WeatherCode.SnowShowersOrIntermittentSnow, PackIconKind.WeatherPartlySnowy },
            { WeatherCode.Hail, PackIconKind.WeatherHail },
            { WeatherCode.Thunder, PackIconKind.WeatherThunder },
            { WeatherCode.Pending, PackIconKind.WeatherSunnyOff },
            { WeatherCode.Unknown, PackIconKind.WeatherSunnyOff },
            { WeatherCode.Missing, PackIconKind.WeatherSunnyOff },
        };

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is WeatherCode weatherCode)
            {
                if (this._weatherIconMap.TryGetValue(weatherCode, out var iconKind))
                {
                    return iconKind;
                }
            }

            return PackIconKind.WeatherSunnyOff;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
