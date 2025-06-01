using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using ClockWidget.Logging;
using ClockWidget.Models.Net;
using Microsoft.Extensions.Logging;

namespace ClockWidget.Models.Weather.Amedas
{
    public class AmedasApiClient : RetryableApiClientBase
    {
        private const string AMEDAS_API_URL = "https://www.jma.go.jp/bosai/amedas/data/map/";

        public AmedasApiClient(ILogger<AmedasApiClient> logger, HttpClient httpClient)
            : base(logger, httpClient)
        {
        }

        public async Task<IEnumerable<AmedasData>> GetAllAmedasData10mAsync()
        {
            using var _ = new LoggerScope(this._logger);

            var now = DateTime.Now;

            for (int i = 0; i < 3; i++)
            {
                var target = $"{now.AddMinutes(-10 * i).ToString("yyyyMMddHHmm")[..11]}000.json";
                var url = $"{AMEDAS_API_URL}{target}";

                try
                {
                    this._logger.LogInformation("10分間天気データ（{Target}）取得開始", target);

                    this._logger.LogInformation("{Url} アクセス実行", url);

                    using var res = await this.GetWithRetryAsync(url, HttpCompletionOption.ResponseHeadersRead);
                    res.EnsureSuccessStatusCode();

                    this._logger.LogInformation("{Url} アクセス成功", url);

                    this._logger.LogDebug("{Url} レスポンス取得", url);

                    var json = await res.Content.ReadAsStringAsync();

                    this._logger.LogDebug("JSON デシリアライズ");

                    var amedasDataRaw = JsonSerializer.Deserialize<IDictionary<string, Json.Data.AmedasData>>(json);
                    var amedasData = amedasDataRaw.MapToAmedasData();

                    this._logger.LogInformation("10分間天気データ（{Target}）取得完了", target);

                    return amedasData;
                }
                catch (HttpRequestException ex)
                {
                    if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        this._logger.LogWarning("10分間天気データ（{Target}）未配信のため再取得", target);
                        continue;
                    }

                    this._logger.LogError(ex, "10分間天気データ取得失敗（{Url} アクセス失敗）", url);
                    break;
                }
                catch (JsonException ex)
                {
                    this._logger.LogError(ex, "10分間天気データ取得失敗（JSON デシリアライズ失敗）");
                    break;
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, "10分間天気データ取得失敗（不明なエラー）");
                    break;
                }
            }

            return Enumerable.Empty<AmedasData>();
        }

        public async Task<IEnumerable<AmedasData>> GetAllAmedasData1hAsync()
        {
            using var _ = new LoggerScope(this._logger);

            var now = DateTime.Now;

            for (int i = 0; i < 2; i++)
            {
                var target = $"{now.AddHours(-1 * i):yyyyMMddHH0000}.json";
                var url = $"{AMEDAS_API_URL}{target}";

                try
                {
                    this._logger.LogInformation("1時間天気データ（{Target}）取得開始", target);

                    this._logger.LogInformation("{Url} アクセス実行", url);

                    using var res = await this.GetWithRetryAsync(url, HttpCompletionOption.ResponseHeadersRead);
                    res.EnsureSuccessStatusCode();

                    this._logger.LogInformation("{Url} アクセス成功", url);

                    this._logger.LogDebug("{Url} レスポンス取得", url);

                    var json = await res.Content.ReadAsStringAsync();

                    this._logger.LogDebug("JSON デシリアライズ");

                    var amedasDataRaw = JsonSerializer.Deserialize<IDictionary<string, Json.Data.AmedasData>>(json);
                    var amedasData = amedasDataRaw.MapToAmedasData();

                    this._logger.LogInformation("1時間天気データ（{Target}）取得完了", target);

                    return amedasData;
                }
                catch (HttpRequestException ex)
                {
                    if (ex.StatusCode == System.Net.HttpStatusCode.NotFound)
                    {
                        this._logger.LogWarning("1時間天気データ（{Target}）未配信のため再取得", target);
                        continue;
                    }

                    this._logger.LogError(ex, "1時間天気データ取得失敗（{Url} アクセス失敗）", url);
                    break;
                }
                catch (JsonException ex)
                {
                    this._logger.LogError(ex, "1時間天気データ取得失敗（JSON デシリアライズ失敗）");
                    break;
                }
                catch (Exception ex)
                {
                    this._logger.LogError(ex, "1時間天気データ取得失敗（不明なエラー）");
                    break;
                }
            }

            return Enumerable.Empty<AmedasData>();
        }

        public async Task<AmedasData> GetAmedasData10mByLocationIdAsync(string locationId)
        {
            using var _ = new LoggerScope(this._logger);

            var amedasData = await this.GetAllAmedasData10mAsync();
            return amedasData.FirstOrDefault(x => x.LocationId == locationId);
        }

        public async Task<AmedasData> GetAmedasData1hByLocationIdAsync(string locationId)
        {
            using var _ = new LoggerScope(this._logger);

            var amedasData = await this.GetAllAmedasData1hAsync();
            return amedasData.FirstOrDefault(x => x.LocationId == locationId);
        }
    }
}
