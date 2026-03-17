using System.Collections.Concurrent;

namespace GPS_Listener.Models
{
    public static class SessionManager
    {
        public static ConcurrentDictionary<string, DeviceSession> Sessions
            = new ConcurrentDictionary<string, DeviceSession>();
    }
}