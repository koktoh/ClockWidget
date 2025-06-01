using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ClockWidget.Events;
using Microsoft.Extensions.Logging;
using Prism.Events;

namespace ClockWidget.Models.Initialization
{
    internal class Initializer : AsyncInitializableBase
    {
        private readonly IEventAggregator _eventAggregator;
        private readonly IEnumerable<IAsyncInitializable> _initializables;

        public Initializer(ILogger<Initializer> logger, IEnumerable<InitializableEntry> entries, IEventAggregator eventAggregator)
            : base(logger)
        {
            this._eventAggregator = eventAggregator;

            this._initializables = entries
                .Where(x => x.ShouldInitialize)
                .OrderBy(x => x.Priority)
                .Select(x => x.Instance);

            eventAggregator.GetEvent<SessionActiveEvent>()
                .Subscribe(async () => await this.InitializeAsync(), ThreadOption.BackgroundThread);

            eventAggregator.GetEvent<SessionLogonEvent>()
                .Subscribe(async () => await this.InitializeAsync(), ThreadOption.BackgroundThread);
            eventAggregator.GetEvent<SessionUnlockEvent>()
                .Subscribe(async () => await this.InitializeAsync(), ThreadOption.BackgroundThread);
        }

        protected override async Task InitializeInternalAsync()
        {
            if (this.IsInitialized) return;

            this._logger.LogInformation("起動時初期化開始");

            foreach (var initializable in this._initializables)
            {
                await initializable.InitializeAsync();
            }

            this.SetInitialized();

            this._logger.LogInformation("起動時初期化完了");

            this._eventAggregator.GetEvent<StartupInitializeCompletedEvent>().Publish();
        }

    }
}
