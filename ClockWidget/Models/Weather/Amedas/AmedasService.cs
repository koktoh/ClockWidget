using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using ClockWidget.Events;
using ClockWidget.Logging;
using ClockWidget.Models.Geo;
using ClockWidget.Models.Initialization;
using ClockWidget.Models.Net;
using ClockWidget.Models.Repository;
using Microsoft.Extensions.Logging;
using Prism.Events;

namespace ClockWidget.Models.Weather.Amedas
{
    internal class AmedasService : AsyncInitializableBase, IWeatherService, IAsyncInitializable
    {
        private const int RETRY_COUNT = 3; // リトライ回数

        private const int AMEDAS_DATA_10M_UPDATE_INTERVAL = 5; // 分
        private const int AMEDAS_DATA_1H_UPDATE_INTERVAL = 30; // 分

        private readonly INetworkAccessPolicyService _networkAccessPolicyService;
        private readonly IGeoService _geoService;
        private readonly AmedasApiClient _amedasClient;
        private readonly IRepository<AmedasLocation> _locationRepository;

        // _semaphoreAmedas10m -> _semaphoreAmedas1h の順で取得する（デッドロック対策）
        private readonly SemaphoreSlim _semaphoreAmedas10m = new(1, 1);
        private readonly SemaphoreSlim _semaphoreAmedas1h = new(1, 1);

        private AmedasData _cacheAmedasData10m;
        private AmedasData _cacheAmedasData1h;

        private DateTime _lastAmedasData10mUpdated = DateTime.MinValue;
        private DateTime _lastAmedasData1hUpdated = DateTime.MinValue;

        private string _target10mAmedasLocationId = string.Empty;
        private string _target1hAmedasLocationId = string.Empty;

        public AmedasService(ILogger<AmedasService> logger, INetworkAccessPolicyService networkAccessPolicyService, IGeoService geoService, AmedasApiClient amedasClient, IRepository<AmedasLocation> locationRepository, IEventAggregator eventAggregator)
            : base(logger)
        {
            this._networkAccessPolicyService = networkAccessPolicyService;
            this._geoService = geoService;
            this._amedasClient = amedasClient;
            this._locationRepository = locationRepository;

            eventAggregator.GetEvent<InitializeRequiredEvent>().Subscribe(async type =>
            {
                if (type == this.GetType())
                {
                    await this.InitializeAsync();
                }
            },
            ThreadOption.BackgroundThread);
        }

        protected override async Task InitializeInternalAsync()
        {
            using var _ = new LoggerScope(this._logger);

            if (!this._networkAccessPolicyService.IsAllowedService(this)) return;

            if (this.IsInitialized) return;

            for (int i = 0; i < RETRY_COUNT; i++)
            {
                if (this.IsInitialized) return;

                this._logger.LogInformation("初期化開始（{AttemptCount}回目）", i + 1);

                try
                {
                    var now = DateTime.Now;

                    this._logger.LogDebug("緯度経度データ取得");
                    var geoCoordinate = await this._geoService.GetGeoDataAsync();

                    this._logger.LogDebug("アメダス観測所データ取得");
                    var amedasLocations = await this._locationRepository.GetRecordsAsync();
                    this._logger.LogDebug("10分間天気データ取得");
                    var amedasData10m = await this._amedasClient.GetAllAmedasData10mAsync();
                    this._logger.LogDebug("1時間天気データ取得");
                    var amedasData1h = await this._amedasClient.GetAllAmedasData1hAsync();

                    var amedas = amedasLocations
                        .Select(x => new
                        {
                            x.LocationId,
                            Amedas = x,
                            Data10m = amedasData10m.FirstOrDefault(y => y.LocationId == x.LocationId),
                            Data1h = amedasData1h.FirstOrDefault(y => y.LocationId == x.LocationId),
                            Distance = geoCoordinate.DistanceTo(x.Latitude, x.Longitude)
                        })
                        .OrderBy(x => x.Distance);

                    var data10m = amedas.FirstOrDefault(x => x.Data10m is not null);
                    var data1h = amedas.FirstOrDefault(x => x.Data1h?.Weather is not null);

                    // _semaphoreAmedas10m -> _semaphoreAmedas1h の順で取得する（デッドロック対策）
                    await this._semaphoreAmedas10m.WaitAsync();
                    await this._semaphoreAmedas1h.WaitAsync();
                    try
                    {
                        this._target10mAmedasLocationId = data10m?.LocationId;
                        this._target1hAmedasLocationId = data1h?.LocationId;

                        this._cacheAmedasData10m = data10m?.Data10m;
                        this._lastAmedasData10mUpdated = now;

                        this._cacheAmedasData1h = data1h?.Data1h;
                        this._lastAmedasData1hUpdated = now;
                    }
                    finally
                    {
                        // 逆順に開放（デッドロック対策）
                        this._semaphoreAmedas1h.Release();
                        this._semaphoreAmedas10m.Release();
                    }

                    this.SetInitialized();

                    this._logger.LogInformation("初期化完了");

                    return;
                }
                catch (Exception ex)
                {
                    if(i == RETRY_COUNT - 1)
                    {
                        this._logger.LogError(ex, "初期化失敗（最終試行）");
                        break;
                    }

                    var retryDelaySeconds = Math.Min((int)Math.Pow(2, i), 30);

                    this._logger.LogError(ex, "初期化失敗（{Delay}秒後に再試行）", retryDelaySeconds);

                    await Task.Delay(TimeSpan.FromSeconds(retryDelaySeconds));
                }
            }
        }

        public async Task<Weather> GetWeatherDataAsync()
        {
            using var _ = new LoggerScope(this._logger);

            if (!this.IsInitialized) return new Weather();
            if (!this._networkAccessPolicyService.IsAllowedService(this)) return new Weather();

            // 10分間データ -> 1時間データの順で取得する（デッドロック対策）
            var amedasData10m = await this.GetAmedasData10mAsync();
            var amedasData1h = await this.GetAmedasData1hAsync();

            return new Weather
            {
                Temperature = amedasData10m?.Temperature.Data,
                WeatherCode = amedasData1h?.Weather.Data ?? WeatherCode.Unknown,
            };
        }

        private async Task<AmedasData> GetAmedasData10mAsync()
        {
            using var _ = new LoggerScope(this._logger);

            await this._semaphoreAmedas10m.WaitAsync();
            try
            {
                var now = DateTime.Now;

                if (this._lastAmedasData10mUpdated.AddMinutes(AMEDAS_DATA_10M_UPDATE_INTERVAL) < now)
                {
                    this._logger.LogDebug("API から10分間天気データ取得");

                    this._cacheAmedasData10m = await this._amedasClient.GetAmedasData10mByLocationIdAsync(this._target10mAmedasLocationId);
                    this._lastAmedasData10mUpdated = now;
                }

                return this._cacheAmedasData10m;
            }
            finally
            {
                this._semaphoreAmedas10m.Release();
            }
        }

        private async Task<AmedasData> GetAmedasData1hAsync()
        {
            using var _ = new LoggerScope(this._logger);

            await this._semaphoreAmedas1h.WaitAsync();
            try
            {
                var now = DateTime.Now;

                if (this._lastAmedasData1hUpdated.AddMinutes(AMEDAS_DATA_1H_UPDATE_INTERVAL) < now)
                {
                    this._logger.LogDebug("API から1時間天気データ取得");

                    this._cacheAmedasData1h = await this._amedasClient.GetAmedasData1hByLocationIdAsync(this._target1hAmedasLocationId);
                    this._lastAmedasData1hUpdated = now;
                }

                return this._cacheAmedasData1h;
            }
            finally
            {
                this._semaphoreAmedas1h.Release();
            }
        }
    }
}
