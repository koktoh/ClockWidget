using System.Collections.Generic;

namespace ClockWidget.Models.Weather.Amedas
{
    public static class AmedasMapper
    {
        public static AmedasLocation MapToAmedasLocation(this KeyValuePair<string, Json.Location.AmedasLocation> location)
        {
            return new AmedasLocation
            {
                LocationId = location.Key,
                Type = location.Value.Type,
                Elems = location.Value.Elems,
                Latitude = location.Value.Latitude.ToGeoCoordinate(),
                Longitude = location.Value.Longitude.ToGeoCoordinate(),
                Alt = location.Value.Alt,
                KanjiName = location.Value.KanjiName,
                KanaName = location.Value.KanaName,
                EnName = location.Value.EnName,
            };
        }

        public static AmedasData MapToAmedasData(this KeyValuePair<string, Json.Data.AmedasData> data)
        {
            return new AmedasData
            {
                LocationId = data.Key,
                Pressure = data.Value.Pressure,
                NormalPressure = data.Value.NormalPressure,
                Temperature = data.Value.Temperature,
                Humidity = data.Value.Humidity,
                Visiblity = data.Value.Visiblity,
                Snow = data.Value.Snow,
                Weather = data.Value.Weather,
                Snow1h = data.Value.Snow1h,
                Snow6h = data.Value.Snow6h,
                Snow12h = data.Value.Snow12h,
                Snow24h = data.Value.Snow24h,
                Sun10m = data.Value.Sun10m,
                Sun1h = data.Value.Sun1h,
                Precipitation10m = data.Value.Precipitation10m,
                Precipitation1h = data.Value.Precipitation1h,
                Precipitation3h = data.Value.Precipitation3h,
                Precipitation24h = data.Value.Precipitation24h,
                WindDirection = data.Value.WindDirection,
                Wind = data.Value.Wind,
            };
        }

        public static IEnumerable<AmedasLocation> MapToAmedasLocations(this IDictionary<string, Json.Location.AmedasLocation> locations)
        {
            foreach (var location in locations)
            {
                yield return location.MapToAmedasLocation();
            }
        }

        public static IEnumerable<AmedasData> MapToAmedasData(this IDictionary<string, Json.Data.AmedasData> data)
        {
            foreach (var item in data)
            {
                yield return item.MapToAmedasData();
            }
        }
    }
}
