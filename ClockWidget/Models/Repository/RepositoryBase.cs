using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using ClockWidget.Logging;
using ClockWidget.Models.Net;
using Microsoft.Extensions.Logging;

namespace ClockWidget.Models.Repository
{
    public abstract class RepositoryBase<T> : IRepository<T>
    {
        private readonly int _cacheDays;

        protected readonly string _cacheFileDirectory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
        protected readonly string _cacheFileName;
        protected readonly string _cacheFilePath;

        protected readonly ILogger _logger;
        protected readonly INetworkAccessPolicyService _networkAccessPolicyService;

        protected RepositoryBase(ILogger logger, INetworkAccessPolicyService networkAccessPolicyService, string fileName, int cacheDays)
        {
            this._logger = logger;
            this._networkAccessPolicyService = networkAccessPolicyService;
            this._cacheDays = cacheDays;
            this._cacheFileName = fileName;
            this._cacheFilePath = Path.Combine(this._cacheFileDirectory, this._cacheFileName);
        }

        public virtual async Task<IEnumerable<T>> GetRecordsAsync()
        {
            using var _ = new LoggerScope(this._logger);

            if (this.ShouldFetchFromApi() && this._networkAccessPolicyService.IsAllowed())
            {
                this._logger.LogDebug("API からデータ取得");

                var records = await this.FetchFromApiAsync();
                await this.SaveToLocalAsync(records);
                return records;
            }

            this._logger.LogDebug("ローカルからデータ取得");
            return await this.LoadFromLocalAsync();
        }

        protected abstract Task<IEnumerable<T>> FetchFromApiAsync();

        protected virtual async Task<IEnumerable<T>> LoadFromLocalAsync()
        {
            using var _ = new LoggerScope(this._logger);

            if (!File.Exists(this._cacheFilePath)) return Enumerable.Empty<T>();

            try
            {
                var json = await File.ReadAllTextAsync(this._cacheFilePath);
                var data = JsonSerializer.Deserialize<IEnumerable<T>>(json);
                return data ?? Enumerable.Empty<T>();
            }
            catch (JsonException ex)
            {
                this._logger.LogError(ex, "JSON デシリアライズ失敗");
                return Enumerable.Empty<T>();
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "不明なエラー");
                return Enumerable.Empty<T>();
            }
        }

        protected virtual async Task SaveToLocalAsync(IEnumerable<T> records)
        {
            using var _ = new LoggerScope(this._logger);

            try
            {
                if (!Directory.Exists(this._cacheFileDirectory))
                {
                    Directory.CreateDirectory(this._cacheFileDirectory);
                }

                var json = JsonSerializer.Serialize(records, new JsonSerializerOptions { WriteIndented = true });
                await File.WriteAllTextAsync(this._cacheFilePath, json, System.Text.Encoding.UTF8);
            }
            catch (Exception ex)
            {
                this._logger.LogError(ex, "ローカルファイル保存失敗");
            }
        }

        protected bool ShouldFetchFromApi()
        {
            if (!File.Exists(this._cacheFilePath)) return true;
            var lastModified = File.GetLastWriteTime(this._cacheFilePath);
            return (DateTime.Now - lastModified).TotalDays > this._cacheDays;
        }
    }
}
