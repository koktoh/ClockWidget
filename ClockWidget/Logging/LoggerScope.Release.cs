#if !DEBUG

using System;

namespace ClockWidget.Logging
{
    public struct LoggerScope : IDisposable
    {
        public LoggerScope(object _ = null) { }
        public void Dispose() { }
    }
}

#endif
