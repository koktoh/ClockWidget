using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ClockWidget.Logging;
using ClockWidget.Models.Net;
using Microsoft.Extensions.Logging;

namespace ClockWidget.Models.Weather.Amedas
{
    public class AmedasLocationApiClient : RetryableApiClientBase
    {
        private const string AMEDAS_TABLE_URL = "https://www.jma.go.jp/bosai/amedas/const/amedastable.json";

        public AmedasLocationApiClient(ILogger<AmedasLocationApiClient> logger, HttpClient httpClient)
            : base(logger, httpClient)
        {
        }

        public async Task<IDictionary<string, Json.Location.AmedasLocation>> GetAmedasLocationsAsync()
        {
            using var _ = new LoggerScope(this._logger);

            try
            {
                this._logger.LogInformation("アメダス観測所データ取得開始");

                this._logger.LogInformation("{Url} アクセス実行", AMEDAS_TABLE_URL);

                using var res = await this.GetWithRetryAsync(AMEDAS_TABLE_URL, HttpCompletionOption.ResponseHeadersRead);
                res.EnsureSuccessStatusCode();

                this._logger.LogInformation("{Url} アクセス成功", AMEDAS_TABLE_URL);

                this._logger.LogDebug("{Url} レスポンス取得", AMEDAS_TABLE_URL);

                var json = await res.Content.ReadAsStringAsync();

                this._logger.LogDebug("JSON デシリアライズ");

                var amedasLocations = JsonSerializer.Deserialize<IDictionary<string, Json.Location.AmedasLocation>>(json);

                this._logger.LogInformation("アメダス観測所データ取得完了");

                return amedasLocations;
            }
            catch (HttpRequestException ex)
            {
                this._logger.LogError(ex, "アメダス観測所データ取得失敗（{Url} アクセス失敗）", AMEDAS_TABLE_URL);
                return new Dictionary<string, Json.Location.AmedasLocation>();
            }
            catch (JsonException ex)
            {
                this._logger.LogError(ex, "アメダス観測所データ取得失敗（JSON デシリアライズ失敗）");
                return new Dictionary<string, Json.Location.AmedasLocation>();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "アメダス観測所データ取得失敗（不明なエラー）");
                return new Dictionary<string, Json.Location.AmedasLocation>();
            }
        }
    }
}
