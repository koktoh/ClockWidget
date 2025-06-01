using System.Collections.Generic;
using System.Threading.Tasks;
using ClockWidget.Logging;
using ClockWidget.Models.Net;
using ClockWidget.Models.Repository;
using Microsoft.Extensions.Logging;

namespace ClockWidget.Models.Holiday.JP
{
    internal class JpHolidayRepository : RepositoryBase<Holiday>, IRepository<Holiday>
    {
        private const string CACHE_FILE_NAME = "jp_holiday.json";
        private const int CACHE_DAYS = 180;

        private readonly IHolidayApiClient _holidayApiClient;

        public JpHolidayRepository(ILogger<JpHolidayRepository> logger, INetworkAccessPolicyService networkAccessPolicyService, IHolidayApiClient holidayApiClient)
            : base(logger, networkAccessPolicyService, CACHE_FILE_NAME, CACHE_DAYS)
        {
            this._holidayApiClient = holidayApiClient;
        }

        protected override async Task<IEnumerable<Holiday>> FetchFromApiAsync()
        {
            using var _ = new LoggerScope(this._logger);

            return await this._holidayApiClient.GetHolidaysAsync();
        }
    }
}
