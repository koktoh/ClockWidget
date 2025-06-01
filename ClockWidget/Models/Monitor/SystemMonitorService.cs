using System;
using System.Runtime.InteropServices;
using System.Windows.Interop;
using ClockWidget.Events;
using Microsoft.Extensions.Logging;
using Prism.Events;

namespace ClockWidget.Models.Monitor
{
    internal class SystemMonitorService : ISystemMonitorService, IDisposable
    {
        private const int DEBOUNCE_INTERVAL = 500; // ミリ秒

        private readonly ILogger _logger;
        private readonly IEventAggregator _eventAggregator;

        private bool _disposedValue;

        private HwndSource _hwndSource;
        private IntPtr _hwnd;

        private int _lastPowerMode = -1;
        private int _lastSessionChange = -1;
        private DateTime _lastPowerModeChangeTime = DateTime.MinValue;
        private DateTime _lastSessionChangeTime = DateTime.MinValue;

        public SystemMonitorService(ILogger<SystemMonitorService> logger, IEventAggregator eventAggregator)
        {
            this._logger = logger;
            this._eventAggregator = eventAggregator;
        }

        public void Initialize()
        {
            var window = System.Windows.Application.Current.MainWindow ?? throw new InvalidOperationException("MainWindow が初期化されていません");

            if (SystemMonitor.IsInteractiveSessionActive())
            {
                this._logger.LogInformation("起動時セッション状態: アクティブ");
                this._eventAggregator.GetEvent<SessionActiveEvent>().Publish();
            }
            else
            {
                this._logger.LogInformation("起動時セッション状態: 非アクティブ");
            }

            this._hwnd = new WindowInteropHelper(window).Handle;
            this._hwndSource = HwndSource.FromHwnd(this._hwnd);
            this._hwndSource?.AddHook(this.WndProc);
            WTSRegisterSessionNotification(this._hwnd, NOTIFY_FOR_THIS_SESSION);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this._disposedValue)
            {
                if (disposing)
                {
                    if (this._hwnd != IntPtr.Zero)
                    {
                        WTSUnRegisterSessionNotification(this._hwnd);
                    }

                    if (this._hwndSource is not null)
                    {
                        this._hwndSource.RemoveHook(this.WndProc);
                        this._hwndSource.Dispose();
                        this._hwndSource = null;
                    }
                }

                this._disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        private IntPtr WndProc(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            switch (msg)
            {
                case WM_POWERBROADCAST:
                    this.HandlePowerModeChange((int)wParam);
                    break;
                case WM_WTSSESSION_CHANGE:
                    this.HandleSessionChange((int)wParam);
                    break;
            }

            return IntPtr.Zero;
        }

        private void HandlePowerModeChange(int mode)
        {
            var now = DateTime.UtcNow;

            if (this._lastPowerMode == mode && (now - this._lastPowerModeChangeTime).TotalMilliseconds < DEBOUNCE_INTERVAL)
            {
                // 同じモードの変更が短時間で連続して発生した場合は無視
                this._logger.LogDebug("同じモードの変更が短時間で発生: {Mode}", mode);
                return;
            }

            this._lastPowerMode = mode;
            this._lastPowerModeChangeTime = now;

            switch (mode)
            {
                case PBT_APMSUSPEND:
                    this._eventAggregator.GetEvent<SystemSuspnedEvent>().Publish();
                    this._logger.LogDebug("システムサスペンド");
                    break;
                case PBT_APMRESUME:
                    this._eventAggregator.GetEvent<SystemResumeEvent>().Publish();
                    this._logger.LogDebug("システムレジューム");
                    break;
            }
        }

        private void HandleSessionChange(int changeType)
        {
            var now = DateTime.UtcNow;

            if (this._lastSessionChange == changeType && (now - this._lastSessionChangeTime).TotalMilliseconds < DEBOUNCE_INTERVAL)
            {
                // 同じセッション変更が短時間で連続して発生した場合は無視
                this._logger.LogDebug("同じセッション変更が短時間で発生: {ChangeType}", changeType);
                return;
            }

            this._lastSessionChange = changeType;
            this._lastSessionChangeTime = now;

            switch (changeType)
            {
                case WTS_SESSION_LOGON:
                    this._eventAggregator.GetEvent<SessionLogonEvent>().Publish();
                    this._logger.LogDebug("セッションログオン");
                    break;
                case WTS_SESSION_LOGOFF:
                    this._eventAggregator.GetEvent<SessionLogoffEvent>().Publish();
                    this._logger.LogDebug("セッションログオフ");
                    break;
                case WTS_SESSION_LOCK:
                    this._eventAggregator.GetEvent<SessionLockEvent>().Publish();
                    this._logger.LogDebug("セッションロック");
                    break;
                case WTS_SESSION_UNLOCK:
                    this._eventAggregator.GetEvent<SessionUnlockEvent>().Publish();
                    this._logger.LogDebug("セッションアンロック");
                    break;
            }
        }

        private const int WM_POWERBROADCAST = 0x0218;
        private const int PBT_APMSUSPEND = 0x0004;
        private const int PBT_APMRESUME = 0x0007;

        private const int WM_WTSSESSION_CHANGE = 0x02B1;
        private const int WTS_SESSION_LOGON = 0x0005;
        private const int WTS_SESSION_LOGOFF = 0x0006;
        private const int WTS_SESSION_LOCK = 0x0007;
        private const int WTS_SESSION_UNLOCK = 0x0008;

        private const int NOTIFY_FOR_THIS_SESSION = 0x0000;

        [DllImport("Wtsapi32.dll")]
        private static extern IntPtr WTSRegisterSessionNotification(IntPtr hWnd, int dwFlags);

        [DllImport("Wtsapi32.dll")]
        private static extern bool WTSUnRegisterSessionNotification(IntPtr hWnd);
    }
}
