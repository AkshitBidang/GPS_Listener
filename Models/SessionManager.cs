//using System.Collections.Concurrent;

//namespace GPS_Listener.Models
//{
//    public static class SessionManager
//    {
//        public static ConcurrentDictionary<string, DeviceSession> Sessions
//            = new ConcurrentDictionary<string, DeviceSession>();
//    }
//}



using System.Collections.Concurrent;
using System.Net.Sockets;

namespace GPS_Listener.Models
{
    public static class SessionManager
    {
        // ✅ IMEI → Session
        public static ConcurrentDictionary<string, DeviceSession> Sessions
            = new ConcurrentDictionary<string, DeviceSession>();

        // 🔥 NEW: TcpClient → IMEI (fast lookup)
        public static ConcurrentDictionary<TcpClient, string> ClientMap
            = new ConcurrentDictionary<TcpClient, string>();
    }
}