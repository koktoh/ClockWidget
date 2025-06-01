using System;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Windows;
using ClockWidget.Logging;
using ClockWidget.Models;
using ClockWidget.Models.Geo;
using ClockWidget.Models.Holiday;
using ClockWidget.Models.Holiday.JP;
using ClockWidget.Models.Initialization;
using ClockWidget.Models.Monitor;
using ClockWidget.Models.Net;
using ClockWidget.Models.Repository;
using ClockWidget.Models.Setting;
using ClockWidget.Models.Weather;
using ClockWidget.Models.Weather.Amedas;
using ClockWidget.Views;
using DryIoc;
using Microsoft.Extensions.Logging;
using Prism.Container.DryIoc;
using Prism.Ioc;
using Serilog;

namespace ClockWidget
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App
    {
        public static ILoggerFactory LoggerFactory { get; private set; }

        private Microsoft.Extensions.Logging.ILogger _logger;
        private IReadonlySetting _setting;

        protected override Window CreateShell()
        {
            return Container.Resolve<MainWindow>();
        }

        protected override void RegisterTypes(IContainerRegistry containerRegistry)
        {
            var container = containerRegistry.GetContainer();

            container.Use(LoggerFactory);
            container.Register(typeof(ILogger<>),
                               made: Made.Of(request => typeof(LoggerFactoryExtensions)
                                                          .GetMethod(nameof(LoggerFactoryExtensions.CreateLogger),
                                                                     BindingFlags.Static | BindingFlags.Public,
                                                                     null,
                                                                     [typeof(ILoggerFactory)],
                                                                     null)
                                                          .MakeGenericMethod(request.Parent.ImplementationType)),
                               reuse: Reuse.Singleton);

            containerRegistry.RegisterSingleton<ISystemMonitorService, SystemMonitorService>();
            containerRegistry.RegisterSingleton<ISettingService, SettingService>();
            containerRegistry.RegisterSingleton<INetworkAccessibilityService, NetworkAccessibilityService>();
            containerRegistry.RegisterSingleton<INetworkAccessPolicyService, NetworkAccessPolicyService>();
            containerRegistry.RegisterSingleton<HttpClient>();
            containerRegistry.RegisterSingleton<IHolidayApiClient, JpHolidayApiClient>();
            containerRegistry.RegisterSingleton<IRepository<Holiday>, JpHolidayRepository>();
            containerRegistry.RegisterSingletonWithInitializable<IHolidayService, JpHolidayService>(priority: 1);
            containerRegistry.RegisterSingleton<GeoApiClient>();
            containerRegistry.RegisterSingleton<IGeoService, GeoService>();
            containerRegistry.RegisterSingleton<AmedasApiClient>();
            containerRegistry.RegisterSingleton<IRepository<AmedasLocation>, AmedasLocationRepository>();
            containerRegistry.RegisterSingletonWithInitializable<IWeatherService, AmedasService>(priority: 1, shouldInitialize: !this._setting.StandAlone && this._setting.ShowWeather);
            containerRegistry.RegisterSingleton<Initializer>();
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            this._setting = SettingReader.Read(SettingService.SettingFilePath);

            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .Enrich.WithProperty("Application", Assembly.GetExecutingAssembly().GetName().Name)
                .Enrich.With(new ClassNameEnricher())
                .WriteTo.Debug(outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{SourceContext}] {Message:lj}{NewLine}{Exception}")
                .WriteTo.File("logs/log.txt",
                              restrictedToMinimumLevel: Serilog.Events.LogEventLevel.Information,
                              outputTemplate: "[{Timestamp:yyyy-MM-dd HH:mm:ss} {Level:u3}] [{ClassName}] {Message:lj}{NewLine}{Exception}",
                              rollingInterval: RollingInterval.Day,
                              retainedFileTimeLimit: TimeSpan.FromDays(7))
                .CreateLogger();

            LoggerFactory = Microsoft.Extensions.Logging.LoggerFactory.Create(builder =>
            {
                builder.ClearProviders();
                builder.AddSerilog();
            });

            this._logger = LoggerFactory.CreateLogger<App>();

            this._logger.LogInformation("{Application} 起動（Version: {Version}）", Assembly.GetExecutingAssembly().GetName().Name, Assembly.GetExecutingAssembly().GetName().Version);
            this._logger.LogInformation("OS: {OS}",Environment.OSVersion);
            this._logger.LogInformation("スタンドアロンモード: {State}", this._setting.StandAlone ? "YES" : "NO");
            this._logger.LogInformation("日付表示: {State}", this._setting.ShowDate ? "YES" : "NO");
            this._logger.LogInformation("天気情報表示: {State}", this._setting.ShowWeather ? "YES" : "NO");

            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                this._logger.LogCritical((Exception)args.ExceptionObject, "不明なエラー");
                MessageBox.Show("不明なエラーが発生しました。詳細はログを確認してください。", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            };

            this.DispatcherUnhandledException += (sender, args) =>
            {
                this._logger.LogCritical(args.Exception, "不明なエラー");
                MessageBox.Show("不明なエラーが発生しました。詳細はログを確認してください。", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                args.Handled = true;
            };

            TaskScheduler.UnobservedTaskException += (sender, args) =>
            {
                this._logger.LogCritical(args.Exception, "不明なエラー");
                MessageBox.Show("不明なエラーが発生しました。詳細はログを確認してください。", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                args.SetObserved();
            };

            base.OnStartup(e);

            // Initializer を最初に解決する（実際の初期化は任意のタイミング）
            _ = Container.Resolve<Initializer>();

            // 遅延初期化するためのイベント購読サービスを初期化
            var monitor = Container.Resolve<ISystemMonitorService>();
            monitor.Initialize();
        }

        protected override void OnExit(ExitEventArgs e)
        {
            this._logger.LogInformation("{Application} 終了", Assembly.GetExecutingAssembly().GetName().Name);
            Log.CloseAndFlush();

            base.OnExit(e);
        }
    }
}
