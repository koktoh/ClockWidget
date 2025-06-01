using System;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;
using ClockWidget.Events;
using ClockWidget.Models.Holiday;
using ClockWidget.Models.Setting;
using ClockWidget.Models.Weather;
using ClockWidget.Views;
using Prism.Commands;
using Prism.Events;
using Prism.Mvvm;

namespace ClockWidget.ViewModels
{
    public class MainWindowViewModel : BindableBase
    {
        private readonly ISettingService _settingService;
        private readonly IHolidayService _holidayService;
        private readonly IWeatherService _weatherService;
        private readonly IEventAggregator _eventAggregator;

        private readonly DispatcherTimer _clockTimer;
        private readonly DispatcherTimer _weatherTimer;

        private bool _isInitialized = false;
        private bool _isWeatherContextMenuItemEnabled = false;

        private Setting _setting;

        private int _size = 0;
        private int _width = 0;
        private int _height = 0;
        private int _center = 0;

        private bool _standAlone = false;

        private bool _topmost = true;
        private bool _showDate = true;
        private bool _showWeather = true;

        private FontFamily _mainFontFamily = new FontFamily("Impact");
        private FontFamily _holidayFontFamily = new FontFamily("Yu Gothic UI Semibold");

        private SolidColorBrush _clockFaceColor = Brushes.White;
        private SolidColorBrush _hourHandColor = Brushes.White;
        private SolidColorBrush _minuteHandColor = Brushes.White;
        private SolidColorBrush _secondHnadColor = Brushes.White;
        private SolidColorBrush _dateColor = Brushes.White;
        private SolidColorBrush _dayOfWeekColor = Brushes.White;

        private DateTime _date = DateTime.Now;
        private string _holidayName;

        private Weather _weather = new Weather();

        private double _hourAngle = 0;
        private double _minuteAngle = 0;
        private double _secondAngle = 0;

        private ICommand _loadCommand;
        private ICommand _toggleStandAloneCommand;
        private ICommand _toggleTopmostCommand;
        private ICommand _toggleShowDateCommand;
        private ICommand _toggleShowWeatherCommand;
        private ICommand _openSettingsWindowCommand;
        private ICommand _closeCommand;
        private ICommand _mouseLeftButtonDownCommand;

        public bool IsInitialized
        {
            get { return this._isInitialized; }
            set
            {
                if (this.SetProperty(ref this._isInitialized, value))
                {
                    this.UpdateIsWeatherContextMenuItemEnabled();
                }
            }
        }

        public bool IsWeatherContextMenuItemEnabled
        {
            get { return this._isWeatherContextMenuItemEnabled; }
            set { this.SetProperty(ref this._isWeatherContextMenuItemEnabled, value); }
        }

        public int Size
        {
            get { return this._size; }
            set
            {
                this.Center = value / 2;
                this.SetProperty(ref this._size, value);
            }
        }

        public int Width
        {
            get { return this._width; }
            set { this.SetProperty(ref this._width, value); }
        }

        public int Height
        {
            get { return this._height; }
            set { this.SetProperty(ref this._height, value); }
        }

        public int Center
        {
            get { return this._center; }
            set { this.SetProperty(ref this._center, value); }
        }

        public double WindowTop
        {
            get { return this._setting.WindowTop; }
            set
            {
                this._setting.WindowTop = value;
                this.RaisePropertyChanged(nameof(this.WindowTop));
            }
        }

        public double WindowLeft
        {
            get { return this._setting.WindowLeft; }
            set
            {
                this._setting.WindowLeft = value;
                this.RaisePropertyChanged(nameof(this.WindowTop));
            }
        }

        public bool StandAlone
        {
            get { return this._standAlone; }
            set
            {
                if (this.SetProperty(ref this._standAlone, value))
                {
                    this.UpdateIsWeatherContextMenuItemEnabled();
                }
            }
        }

        public bool Topmost
        {
            get { return this._topmost; }
            set { this.SetProperty(ref this._topmost, value); }
        }

        public bool ShowDate
        {
            get { return this._showDate; }
            set { this.SetProperty(ref this._showDate, value); }
        }

        public bool ShowWeather
        {
            get { return this._showWeather; }
            set { this.SetProperty(ref this._showWeather, value); }
        }

        public FontFamily MainFontFamily
        {
            get { return this._mainFontFamily; }
            set { this.SetProperty(ref this._mainFontFamily, value); }
        }

        public FontFamily HolidayFontFamily
        {
            get { return this._holidayFontFamily; }
            set { this.SetProperty(ref this._holidayFontFamily, value); }
        }

        public SolidColorBrush ClockFaceColor
        {
            get { return this._clockFaceColor; }
            set { this.SetProperty(ref this._clockFaceColor, value); }
        }

        public SolidColorBrush HourHandColor
        {
            get { return this._hourHandColor; }
            set { this.SetProperty(ref this._hourHandColor, value); }
        }

        public SolidColorBrush MinuteHandColor
        {
            get { return this._minuteHandColor; }
            set { this.SetProperty(ref this._minuteHandColor, value); }
        }

        public SolidColorBrush SecondHandColor
        {
            get { return this._secondHnadColor; }
            set { this.SetProperty(ref this._secondHnadColor, value); }
        }

        public SolidColorBrush DateColor
        {
            get { return this._dateColor; }
            set { this.SetProperty(ref this._dateColor, value); }
        }

        public SolidColorBrush DayOfWeekColor
        {
            get { return this._dayOfWeekColor; }
            set { this.SetProperty(ref this._dayOfWeekColor, value); }
        }

        public DateTime Date
        {
            get { return this._date; }
            set { this.SetProperty(ref this._date, value); }
        }

        public string HolidayName
        {
            get { return this._holidayName; }
            set { this.SetProperty(ref this._holidayName, value); }
        }

        public Weather Weather
        {
            get { return this._weather; }
            set { this.SetProperty(ref this._weather, value); }
        }

        public double HourAngle
        {
            get { return this._hourAngle; }
            set { this.SetProperty(ref this._hourAngle, value); }
        }

        public double MinuteAngle
        {
            get { return this._minuteAngle; }
            set { this.SetProperty(ref this._minuteAngle, value); }
        }

        public double SecondAngle
        {
            get { return this._secondAngle; }
            set { this.SetProperty(ref this._secondAngle, value); }
        }

        public ICommand LoadCommand => this._loadCommand ??= new DelegateCommand(async () => await this.OnLoadAsync());
        public ICommand ToggleStandAloneCommand => this._toggleStandAloneCommand ??= new DelegateCommand(this.OnToggleStandAlone);
        public ICommand ToggleTopmostCommand => this._toggleTopmostCommand ??= new DelegateCommand(this.OnToggleTopmost);
        public ICommand ToggleShowDateCommand => this._toggleShowDateCommand ??= new DelegateCommand(this.OnToggleShowDate);
        public ICommand ToggleShowWeatherCommand => this._toggleShowWeatherCommand ??= new DelegateCommand(this.OnToggleShowWeather);
        public ICommand OpenSettingsWindowCommand => this._openSettingsWindowCommand ??= new DelegateCommand(this.OpenSettingsWindow);
        public ICommand CloseCommand => this._closeCommand ??= new DelegateCommand(this.Close);
        public ICommand MouseLeftButtonDownCommand => this._mouseLeftButtonDownCommand ??= new DelegateCommand(this.OnMouseLeftButtonDown);

        public MainWindowViewModel(ISettingService settingService, IHolidayService holidayService, IWeatherService weatherService, IEventAggregator eventAggregator)
        {
            this._settingService = settingService;

            this._holidayService = holidayService;
            this._weatherService = weatherService;
            this._eventAggregator = eventAggregator;

            this.ApplySetting(settingService.Current.Clone());

            this.WindowTop = this._setting.WindowTop;
            this.WindowLeft = this._setting.WindowLeft;

            this._clockTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromMilliseconds(100)
            };
            this._clockTimer.Tick += (s, e) =>
            {
                this.RefreshClock();
            };

            this._weatherTimer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(5)
            };
            this._weatherTimer.Tick += async (s, e) =>
            {
                await this.RefreshWeatherAsync();
            };

            eventAggregator.GetEvent<StartupInitializeCompletedEvent>()
                .Subscribe(() =>
                {
                    this.IsInitialized = true;
                },
                ThreadOption.UIThread);
            eventAggregator.GetEvent<SettingChangedEvent>().Subscribe(this.HandleSettingChanged, ThreadOption.UIThread);
        }

        private void ApplySetting(Setting setting)
        {
            this._setting = setting;

            this.Size = this._setting.Size;
            this.Width = this.Size;
            this.Height = this.Size;

            this.StandAlone = this._setting.StandAlone;

            this.Topmost = this._setting.Topmost;
            this.ShowDate = this._setting.ShowDate;
            this.ShowWeather = !this._setting.StandAlone && this._setting.ShowWeather;

            this.MainFontFamily = this._setting.MainFontFamily;
            this.HolidayFontFamily = this._setting.HolidayFontFamily;

            this.ClockFaceColor = new SolidColorBrush(this._setting.ClockFaceColor);
            this.HourHandColor = new SolidColorBrush(this._setting.HourHandColor);
            this.MinuteHandColor = new SolidColorBrush(this._setting.MinuteHandColor);
            this.SecondHandColor = new SolidColorBrush(this._setting.SecondHandColor);
            this.DateColor = new SolidColorBrush(this._setting.DateColor);
            this.DayOfWeekColor = this.GetDayOfWeekColor();
        }

        private void HandleSettingChanged(Setting setting)
        {
            this.ApplySetting(setting);

            if (!this.StandAlone && this.ShowDate)
            {
                this._eventAggregator.GetEvent<InitializeRequiredEvent>().Publish(this._weatherService.GetType());
                _ = this.StartWeatherTimer();
            }
            else
            {
                this.StopWeatherTimer();
            }
        }

        private void StartClockTimer()
        {
            if (!this._clockTimer?.IsEnabled ?? false)
            {
                this.RefreshClock();
                this._clockTimer.Start();
            }
        }

        private void StopClockTimer()
        {
            if (this._clockTimer?.IsEnabled ?? false)
            {
                this._clockTimer.Stop();
            }
        }

        private void RefreshClock()
        {
            this.Date = DateTime.Now;

            this.SecondAngle = this.Date.Second * 6;
            this.MinuteAngle = this.Date.Minute * 6 + this.Date.Second * 0.1;
            this.HourAngle = (this.Date.Hour % 12) * 30 + this.Date.Minute * 0.5;

            this.DayOfWeekColor = this.GetDayOfWeekColor();

            this.HolidayName = this.GetHolidayName();
        }

        private async Task StartWeatherTimer()
        {
            if (this._setting.StandAlone || !this._setting.ShowWeather) return;

            if (!this._weatherTimer?.IsEnabled ?? false)
            {
                await this.RefreshWeatherAsync();
                this._weatherTimer.Start();
            }
        }

        private void StopWeatherTimer()
        {
            if (this._weatherTimer?.IsEnabled ?? false)
            {
                this._weatherTimer.Stop();
            }
        }

        private async Task RefreshWeatherAsync()
        {
            this.Weather = await this._weatherService.GetWeatherDataAsync().ConfigureAwait(false);
        }

        private SolidColorBrush GetDayOfWeekColor()
        {
            if (this._holidayService.IsTodayHoliday()) return Brushes.IndianRed;

            return this.Date.DayOfWeek switch
            {
                DayOfWeek.Saturday => Brushes.RoyalBlue,
                DayOfWeek.Sunday => Brushes.IndianRed,
                _ => this.DateColor,
            };
        }

        private string GetHolidayName()
        {
            var holiday = this._holidayService.GetToday();

            if (holiday is null) return string.Empty;

            if (holiday.Name.Equals("休日")) return "振替休日";

            return holiday.Name;
        }

        private void UpdateIsWeatherContextMenuItemEnabled()
        {
            this.IsWeatherContextMenuItemEnabled = this.IsInitialized && !this.StandAlone;
        }

        private async Task OnLoadAsync()
        {
            this.StartClockTimer();
            await this.StartWeatherTimer();
        }

        private void OnToggleStandAlone()
        {
            this._setting.StandAlone = !this._setting.StandAlone;
            this._settingService.Save(this._setting);
        }

        private void OnToggleTopmost()
        {
            this._setting.Topmost = !this._setting.Topmost;
            this._settingService.Save(this._setting);
        }

        private void OnToggleShowDate()
        {
            this._setting.ShowDate = !this._setting.ShowDate;
            this._settingService.Save(this._setting);
        }

        private void OnToggleShowWeather()
        {
            this._setting.ShowWeather = !this._setting.ShowWeather;
            this._settingService.Save(this._setting);
        }

        private void OpenSettingsWindow()
        {
            var vm = new SettingWindowViewModel(this._settingService);
            var settingWindow = new SettingWindow
            {
                DataContext = vm,
                Owner = Application.Current.MainWindow,
                SaveWindowPosition = true,
            };

            settingWindow.ShowDialog();
        }

        private void Close()
        {
            this.StopClockTimer();
            this.StopWeatherTimer();

            Application.Current.Shutdown();
        }

        private void OnMouseLeftButtonDown()
        {
            var window = Application.Current.MainWindow;
            window?.DragMove();
            this.WindowTop = window?.Top ?? 0;
            this.WindowLeft = window?.Left ?? 0;
            this._settingService.Save(this._setting);
        }
    }
}
