using System.Collections.Generic;
using System.Threading.Tasks;
using ClockWidget.Logging;
using ClockWidget.Models.Net;
using ClockWidget.Models.Repository;
using Microsoft.Extensions.Logging;

namespace ClockWidget.Models.Weather.Amedas
{
    internal class AmedasLocationRepository : RepositoryBase<AmedasLocation>, IRepository<AmedasLocation>
    {
        private const string CACHE_FILE_NAME = "amedas_location.json";
        private const int CACHE_DAYS = 180;

        private readonly AmedasLocationApiClient _client;

        public AmedasLocationRepository(ILogger<AmedasLocationRepository> logger, INetworkAccessPolicyService networkAccessPolicyService, AmedasLocationApiClient client)
            : base(logger, networkAccessPolicyService, CACHE_FILE_NAME, CACHE_DAYS)
        {
            this._client = client;
        }

        protected override async Task<IEnumerable<AmedasLocation>> FetchFromApiAsync()
        {
            using var _ = new LoggerScope(this._logger);

            var amedasLocations = await this._client.GetAmedasLocationsAsync();
            return AmedasMapper.MapToAmedasLocations(amedasLocations);
        }
    }
}
