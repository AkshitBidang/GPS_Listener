using System;
using System.Net.Sockets;

namespace GPS_Listener.Models
{
    public class DeviceSession
    {
        public string Imei { get; set; }

        public TcpClient Client { get; set; }

        public DateTime LastHeartbeat { get; set; }

        public DateTime LastLocation { get; set; }

        public bool IsLoggedIn { get; set; }
    }
}