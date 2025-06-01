using System;
using System.Windows.Input;
using System.Windows.Media;
using ClockWidget.Models.Setting;
using Prism.Commands;
using Prism.Mvvm;

namespace ClockWidget.ViewModels
{
    public class SettingWindowViewModel : BindableBase
    {
        private readonly ISettingService _settingService;
        private readonly Setting _setting;
        private readonly IReadonlySetting _defaultSetting;

        private ICommand _okButtonClickCommand;
        private ICommand _cancelButtonClickCommand;
        private ICommand _defaultButtonClickCommand;

        public int Size
        {
            get => this._setting.Size;
            set
            {
                if (this._setting.Size != value)
                {
                    this._setting.Size = value;
                    this.RaisePropertyChanged(nameof(this.Size));
                }
            }
        }

        public bool StandAlone
        {
            get => this._setting.StandAlone;
            set
            {
                if (this._setting.StandAlone != value)
                {
                    this._setting.StandAlone = value;
                    this.RaisePropertyChanged(nameof(this.StandAlone));
                }
            }
        }

        public bool Topmost
        {
            get => this._setting.Topmost;
            set
            {
                if (this._setting.Topmost != value)
                {
                    this._setting.Topmost = value;
                    this.RaisePropertyChanged(nameof(this.Topmost));
                }
            }
        }

        public bool ShowDate
        {
            get => this._setting.ShowDate;
            set
            {
                if (this._setting.ShowDate != value)
                {
                    this._setting.ShowDate = value;
                    this.RaisePropertyChanged(nameof(this.ShowDate));
                }
            }
        }

        public bool ShowWeather
        {
            get => this._setting.ShowWeather;
            set
            {
                if (this._setting.ShowWeather != value)
                {
                    this._setting.ShowWeather = value;
                    this.RaisePropertyChanged(nameof(this.ShowWeather));
                }
            }
        }

        public Color ClockFaceColor
        {
            get => this._setting.ClockFaceColor;
            set
            {
                if (this._setting.ClockFaceColor != value)
                {
                    this._setting.ClockFaceColor = value;
                    this.RaisePropertyChanged(nameof(this.ClockFaceColor));
                }
            }
        }

        public Color HourHandColor
        {
            get => this._setting.HourHandColor;
            set
            {
                if (this._setting.HourHandColor != value)
                {
                    this._setting.HourHandColor = value;
                    this.RaisePropertyChanged(nameof(this.HourHandColor));
                }
            }
        }

        public Color MinuteHandColor
        {
            get => this._setting.MinuteHandColor;
            set
            {
                if (this._setting.MinuteHandColor != value)
                {
                    this._setting.MinuteHandColor = value;
                    this.RaisePropertyChanged(nameof(this.MinuteHandColor));
                }
            }
        }

        public Color SecondHandColor
        {
            get => this._setting.SecondHandColor;
            set
            {
                if (this._setting.SecondHandColor != value)
                {
                    this._setting.SecondHandColor = value;
                    this.RaisePropertyChanged(nameof(this.SecondHandColor));
                }
            }
        }

        public Color DateColor
        {
            get => this._setting.DateColor;
            set
            {
                if (this._setting.DateColor != value)
                {
                    this._setting.DateColor = value;
                    this.RaisePropertyChanged(nameof(this.DateColor));
                }
            }
        }

        public ICommand OkButtonClickCommand => this._okButtonClickCommand ??= new DelegateCommand(this.OkButtonClick);
        public ICommand CancelButtonClickCommand => this._cancelButtonClickCommand ??= new DelegateCommand(this.CancelButtonClick);
        public ICommand DefaultButtonClickCommand => this._defaultButtonClickCommand ??= new DelegateCommand(this.DefaultButtonClick);

        public SettingWindowViewModel(ISettingService settingService)
        {
            this._settingService = settingService;
            this._setting = settingService.Current.Clone();
            this._defaultSetting = settingService.Default.Clone();
        }

        public event Action RequestClose;

        private void OkButtonClick()
        {
            this._settingService.Save(this._setting);
            this.RequestClose?.Invoke();
        }

        private void CancelButtonClick()
        {
            this.RequestClose?.Invoke();
        }

        private void DefaultButtonClick()
        {
            this.Size = this._defaultSetting.Size;

            this.StandAlone = this._defaultSetting.StandAlone;
            this.Topmost = this._defaultSetting.Topmost;
            this.ShowDate = this._defaultSetting.ShowDate;
            this.ShowWeather = this._defaultSetting.ShowWeather;

            this.ClockFaceColor = this._defaultSetting.ClockFaceColor;
            this.HourHandColor = this._defaultSetting.HourHandColor;
            this.MinuteHandColor = this._defaultSetting.MinuteHandColor;
            this.SecondHandColor = this._defaultSetting.SecondHandColor;
            this.DateColor = this._defaultSetting.DateColor;
        }
    }
}
