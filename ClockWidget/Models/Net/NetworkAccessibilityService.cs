using System.Threading;
using ClockWidget.Events;
using Microsoft.Extensions.Logging;
using Prism.Events;

namespace ClockWidget.Models.Net
{
    internal class NetworkAccessibilityService : INetworkAccessibilityService
    {
        public const long ACCESSIBLE = 1;
        public const long NOT_ACCESSIBLE = 0;

        private readonly ILogger _logger;

        private long _accessibility = ACCESSIBLE;

        public bool IsAccessible => Interlocked.Read(ref this._accessibility) == ACCESSIBLE;

        public NetworkAccessibilityService(ILogger<NetworkAccessibilityService> logger, IEventAggregator eventAggregator)
        {
            this._logger = logger;

            eventAggregator.GetEvent<SessionLogonEvent>()
                .Subscribe(() => this.SetAccessibility(true, NetworkAccessibilityChangeReason.Logon), ThreadOption.BackgroundThread);
            eventAggregator.GetEvent<SessionLogoffEvent>()
                .Subscribe(() => this.SetAccessibility(false, NetworkAccessibilityChangeReason.Logoff), ThreadOption.BackgroundThread);

            eventAggregator.GetEvent<SessionUnlockEvent>()
                .Subscribe(() => this.SetAccessibility(true, NetworkAccessibilityChangeReason.Unlock), ThreadOption.BackgroundThread);
            eventAggregator.GetEvent<SessionLockEvent>()
                .Subscribe(() => this.SetAccessibility(false, NetworkAccessibilityChangeReason.Lock), ThreadOption.BackgroundThread);

            eventAggregator.GetEvent<SystemResumeEvent>()
                .Subscribe(() => this.SetAccessibility(true, NetworkAccessibilityChangeReason.SystemResume), ThreadOption.BackgroundThread);
            eventAggregator.GetEvent<SystemSuspnedEvent>()
                .Subscribe(() => this.SetAccessibility(false, NetworkAccessibilityChangeReason.SystemSuspend), ThreadOption.BackgroundThread);
        }

        private void SetAccessibility(bool isAccessible, NetworkAccessibilityChangeReason reason = NetworkAccessibilityChangeReason.Unknown)
        {
            Interlocked.Exchange(ref this._accessibility, isAccessible ? ACCESSIBLE : NOT_ACCESSIBLE);
            this._logger.LogInformation("ネットワークアクセス: {State} （{Reason}）", isAccessible ? "有効" : "無効", reason);
        }
    }
}
