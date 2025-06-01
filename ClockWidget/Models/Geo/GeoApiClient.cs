using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ClockWidget.Logging;
using ClockWidget.Models.Net;
using Microsoft.Extensions.Logging;

namespace ClockWidget.Models.Geo
{
    public class GeoApiClient : RetryableApiClientBase
    {
        private const string GEO_API_URL = "http://ip-api.com/json/?fields=lat,lon";

        public GeoApiClient(ILogger<GeoApiClient> logger, HttpClient httpClient) : base(logger, httpClient)
        {
        }

        public async Task<GeoCoordinate> GetGeoDataAsync()
        {
            using var _ = new LoggerScope(this._logger);

            try
            {
                this._logger.LogInformation("{Url} アクセス実行", GEO_API_URL);

                using var res = await this.GetWithRetryAsync(GEO_API_URL);

                res.EnsureSuccessStatusCode();

                this._logger.LogInformation("{Url} アクセス成功", GEO_API_URL);

                this._logger.LogDebug("{Url} レスポンス取得", GEO_API_URL);

                var json = await res.Content.ReadAsStringAsync();

                this._logger.LogDebug("JSON デシリアライズ");
                return JsonSerializer.Deserialize<GeoCoordinate>(json);
            }
            catch (HttpRequestException ex)
            {
                this._logger.LogError(ex, "{Url} アクセス失敗", GEO_API_URL);
                throw;
            }
            catch (JsonException ex)
            {
                this._logger.LogError(ex, "JSON デシリアライズ失敗");
                throw;
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "不明なエラー");
                throw;
            }
        }
    }
}
