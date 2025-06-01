using System;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Polly;

namespace ClockWidget.Models.Net
{
    public abstract class RetryableApiClientBase
    {
        protected readonly ILogger _logger;
        protected readonly HttpClient _httpClient;
        protected readonly IAsyncPolicy<HttpResponseMessage> _retryPolicy;

        protected RetryableApiClientBase(ILogger logger, HttpClient httpClient)
        {
            this._logger = logger;
            this._httpClient = httpClient;

            this._retryPolicy = Policy
                .Handle<HttpRequestException>()
                .OrResult<HttpResponseMessage>(r => (int)r.StatusCode >= 500)
                .WaitAndRetryAsync(3, retryAttempt =>
                    TimeSpan.FromSeconds(Math.Min((int)Math.Pow(2, retryAttempt), 30)),
                    onRetry: (result, delay, retryCount, context) =>
                    {
                        this._logger.LogWarning("リトライ実行:　リトライ回数 {RetryCount}回, ディレイ {Delay}秒, リトライ原因 {RetryMessage}", retryCount, delay.TotalSeconds, result.Exception?.Message ?? result.Result.StatusCode.ToString());
                    });
        }

        protected async Task<HttpResponseMessage> GetWithRetryAsync(string url)
        {
            return await this._retryPolicy.ExecuteAsync(async () => await this._httpClient.GetAsync(url));
        }

        protected async Task<HttpResponseMessage> GetWithRetryAsync(string url, HttpCompletionOption completionOption)
        {
            return await this._retryPolicy.ExecuteAsync(async () => await this._httpClient.GetAsync(url, completionOption));
        }
    }
}
