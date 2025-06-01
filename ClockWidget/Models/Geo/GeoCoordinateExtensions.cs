using System;

namespace ClockWidget.Models.Geo
{
    public static class GeoCoordinateExtensions
    {
        private const double EARTH_RADIUS = 6371e3; // 地球の半径 (m)

        public static double DistanceTo(this GeoCoordinate source, GeoCoordinate target)
        {
            return source.DistanceTo(target.Latitude, target.Longitude);
        }

        public static double DistanceTo(this GeoCoordinate source, double latitude, double longitude)
        {
            return Haversine(source.Latitude, source.Longitude, latitude, longitude);
        }

        private static double Haversine(double lat1, double lon1, double lat2, double lon2)
        {
            var dLat = (lat2 - lat1) * Math.PI / 180;
            var dLon = (lon2 - lon1) * Math.PI / 180;
            var a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                    Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                    Math.Sin(dLon / 2) * Math.Sin(dLon / 2);
            var c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return EARTH_RADIUS * c; // 距離 (m)
        }
    }
}
