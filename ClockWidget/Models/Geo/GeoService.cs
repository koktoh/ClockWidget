using System.Threading.Tasks;
using ClockWidget.Logging;
using Microsoft.Extensions.Logging;

namespace ClockWidget.Models.Geo
{
    internal class GeoService : IGeoService
    {
        private readonly ILogger _logger;
        private readonly GeoApiClient _geoApiClient;

        public GeoService(ILogger<GeoService> logger, GeoApiClient geoApiClient)
        {
            this._logger = logger;
            this._geoApiClient = geoApiClient;
        }

        public async Task<GeoCoordinate> GetGeoDataAsync()
        {
            using var _ = new LoggerScope(this._logger);

            return await this._geoApiClient.GetGeoDataAsync();
        }
    }
}
