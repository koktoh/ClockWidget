using System.Threading;
using System.Threading.Tasks;
using ClockWidget.Logging;
using Microsoft.Extensions.Logging;

namespace ClockWidget.Models.Initialization
{
    internal abstract class AsyncInitializableBase : IAsyncInitializable
    {
        private const int INITIALIZING = 1;
        private const int NOT_INITIALIZING = 0;

        private const long INITIALIZED = 1;
        private const long NOT_INITIALIZED = 0;

        private int _isInitializing = NOT_INITIALIZING;
        private long _isInitialized = NOT_INITIALIZED;

        protected readonly ILogger _logger;

        public bool IsInitialized => Interlocked.Read(ref this._isInitialized) == INITIALIZED;

        protected AsyncInitializableBase(ILogger logger)
        {
            this._logger = logger;
        }

        public async Task InitializeAsync()
        {
            using var _ = new LoggerScope(this._logger);

            if (Interlocked.CompareExchange(ref this._isInitializing, INITIALIZING, NOT_INITIALIZING) == INITIALIZING) return;

            try
            {
                await this.InitializeInternalAsync();
            }
            finally
            {
                Interlocked.Exchange(ref this._isInitializing, NOT_INITIALIZING);
            }
        }

        protected abstract Task InitializeInternalAsync();

        protected void SetInitialized()
        {
            Interlocked.Exchange(ref this._isInitialized, INITIALIZED);
        }
    }
}
