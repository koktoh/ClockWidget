using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using ClockWidget.Events;
using ClockWidget.Logging;
using Microsoft.Extensions.Logging;
using Prism.Events;

namespace ClockWidget.Models.Setting
{
    internal class SettingService : ISettingService
    {
        public static readonly string SettingDirectory = AppDomain.CurrentDomain.BaseDirectory;
        public static readonly string SettingFilename = "settings.json";
        public static readonly string SettingFilePath = Path.Combine(SettingDirectory, SettingFilename);

        private readonly ILogger _logger;
        private readonly IEventAggregator _eventAggregator;

        private readonly SemaphoreSlim _semaphore = new(1, 1);
        private readonly Setting _defaultSetting = new();

        private Setting _setting;

        public IReadonlySetting Default => this._defaultSetting;

        public IReadonlySetting Current
        {
            get
            {
                this._semaphore.Wait();
                try
                {
                    return this._setting;
                }
                finally
                {
                    this._semaphore.Release();
                }
            }
        }

        public SettingService(ILogger<SettingService> logger, IEventAggregator eventAggregator)
        {
            this._logger = logger;
            this._eventAggregator = eventAggregator;

            this.Load(); // 初期化時に設定を読み込む
        }

        public void Load()
        {
            using var _ = new LoggerScope(this._logger);

            this._semaphore.Wait();
            try
            {
                this._logger.LogInformation("設定ファイルの読み込み開始");

                var setting = SettingReader.Read(SettingFilePath);
                this._setting = (Setting)setting;

                this._logger.LogInformation("設定ファイルの読み込み完了");
            }
            catch (Exception ex)
            {
                this._logger.LogWarning(ex, "設定ファイルの読み込みに失敗");
                this._setting = this._defaultSetting;
            }
            finally
            {
                this._semaphore.Release();
            }
        }

        public async Task LoadAsync()
        {
            using var _ = new LoggerScope(this._logger);

            await this._semaphore.WaitAsync();
            try
            {
                this._logger.LogInformation("設定ファイルの読み込み開始");

                var setting = await SettingReader.ReadAsync(SettingFilePath);
                this._setting = (Setting)setting;

                this._logger.LogInformation("設定ファイルの読み込み完了");
            }
            catch (Exception ex)
            {
                this._logger.LogWarning(ex, "設定ファイルの読み込みに失敗");
                this._setting = this._defaultSetting;
            }
            finally
            {
                this._semaphore.Release();
            }
        }

        public void Save(Setting setting)
        {
            using var _ = new LoggerScope(this._logger);

            this._semaphore.Wait();
            try
            {
                // 変更がない場合は保存しない
                if (setting.Equals(this._setting)) return;

                this._logger.LogInformation("設定ファイルの保存開始");

                SettingWriter.Write(SettingFilePath, setting);
                this._setting = setting.Clone();

                this._logger.LogInformation("設定ファイルの保存完了");

                this._eventAggregator.GetEvent<SettingChangedEvent>().Publish(setting);
            }
            catch (Exception ex)
            {
                this._logger.LogWarning(ex, "設定ファイルの保存に失敗");
            }
            finally
            {
                this._semaphore.Release();
            }
        }

        public async Task SaveAsync(Setting setting)
        {
            using var _ = new LoggerScope(this._logger);

            await this._semaphore.WaitAsync();
            try
            {
                // 変更がない場合は保存しない
                if (setting.Equals(this._setting)) return;

                this._logger.LogInformation("設定ファイルの保存開始");

                await SettingWriter.WriteAsync(SettingFilePath, setting);
                this._setting = setting.Clone();

                this._logger.LogInformation("設定ファイルの保存完了");

                this._eventAggregator.GetEvent<SettingChangedEvent>().Publish(setting);
            }
            catch (Exception ex)
            {
                this._logger.LogWarning(ex, "設定ファイルの保存に失敗");
            }
            finally
            {
                this._semaphore.Release();
            }
        }
    }
}
