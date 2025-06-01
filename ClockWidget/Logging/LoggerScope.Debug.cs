#if DEBUG

using System;
using System.Runtime.CompilerServices;
using Microsoft.Extensions.Logging;

namespace ClockWidget.Logging
{
    public struct LoggerScope : IDisposable
    {
        private readonly ILogger _logger;
        private readonly string _methodName;

        public LoggerScope(ILogger logger, [CallerMemberName] string methodName = "")
        {
            this._logger = logger;
            this._methodName = methodName;

            this._logger.LogDebug("{MethodName} 実行", this._methodName);
        }

        public void Dispose()
        {
            this._logger.LogDebug("{MethodName} 終了", this._methodName);
        }
    }
}

#endif
