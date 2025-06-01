using System;
using System.Runtime.InteropServices;

namespace ClockWidget.Models.Monitor
{
    public static class SystemMonitor
    {
        public static bool IsInteractiveSessionActive()
        {
            int sessionId = WTSGetActiveConsoleSessionId();

            if (sessionId == -1) return false;

            return IsActive(sessionId) && TryGetUserName(sessionId, out _) && !IsLocked(sessionId);
        }

        private static bool IsActive(int sessionId)
        {
            if (WTSQuerySessionInformation(IntPtr.Zero, sessionId, WTS_CONNECT_STATE, out var buffer, out _))
            {
                var state = Marshal.ReadInt32(buffer);
                WTSFreeMemory(buffer);

                return state == WTS_ACTIVE;
            }

            return false;
        }

        private static bool TryGetUserName(int sessionId, out string result)
        {
            result = null;

            if (WTSQuerySessionInformation(IntPtr.Zero, sessionId, WTS_USER_NAME, out var buffer, out _))
            {
                result = Marshal.PtrToStringAnsi(buffer);
                WTSFreeMemory(buffer);

                return true;
            }

            return false;
        }

        private static bool IsLocked(int sessionId)
        {
            if (WinStationQueryInformationW(IntPtr.Zero, sessionId, WIN_STATION_LOCK_STATE, out var isLocked, sizeof(int), out _))
            {
                return isLocked != 0;
            }

            return false;
        }

        private const int WTS_USER_NAME = 5;
        private const int WTS_CONNECT_STATE = 8;

        private const int WTS_ACTIVE = 0;

        private const int WIN_STATION_LOCK_STATE = 28;

        [DllImport("kernel32.dll")]
        private static extern int WTSGetActiveConsoleSessionId();

        [DllImport("Wtsapi32.dll")]
        private static extern bool WTSQuerySessionInformation(IntPtr hServer, int sessionId, int infoClass, out IntPtr buffer, out uint bytesReturned);

        [DllImport("Wtsapi32.dll")]
        private static extern void WTSFreeMemory(IntPtr pointer);

        // 非推奨 API
        // https://learn.microsoft.com/en-us/previous-versions/aa383827(v=vs.85)
        // 使えるので使う
        [DllImport("winsta.dll", CharSet = CharSet.Unicode)]
        private static extern bool WinStationQueryInformationW(IntPtr hServer, int sessionId, int infoClass, out int buffer, int bufferLength, out int returnedLength);
    }
}
