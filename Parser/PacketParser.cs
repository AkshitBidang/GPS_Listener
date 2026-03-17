using System;
using System.Net.Sockets;

//namespace GPS_Listener.Parser
//{
//    public static class PacketParser
//    {
//        public static void Parse(byte[] data, TcpClient client)
//        {
//            if (data.Length < 5)
//            {
//                Console.WriteLine("Invalid Packet");
//                return;
//            }

//            byte protocol = data[3];

//            Console.WriteLine($"Protocol: 0x{protocol:X2}");

//            switch (protocol)
//            {
//                case 0x01:
//                    HandleLogin(data, client);
//                    break;

//                case 0x13:
//                    HandleHeartbeat(data, client);
//                    break;

//                case 0x12:
//                    HandleLocation(data, client);
//                    break;

//                default:
//                    Console.WriteLine("Unknown Packet");
//                    break;
//            }
//        }

//        private static void HandleLogin(byte[] data, TcpClient client)
//        {
//            Console.WriteLine("Login Packet Received");

//            byte[] ack = new byte[]
//            {
//                0x78,0x78,0x05,0x01,0x00,0x01,0xD9,0xDC,0x0D,0x0A
//            };

//            client.GetStream().Write(ack);

//            Console.WriteLine("Login ACK Sent");
//        }

//        private static void HandleHeartbeat(byte[] data, TcpClient client)
//        {
//            Console.WriteLine("Heartbeat Received");

//            byte[] ack = new byte[]
//            {
//                0x78,0x78,0x05,0x13,0x00,0x01,0xD9,0xDC,0x0D,0x0A
//            };

//            client.GetStream().Write(ack);

//            Console.WriteLine("Heartbeat ACK Sent");
//        }

//        private static void HandleLocation(byte[] data, TcpClient client)
//        {
//            Console.WriteLine("Location Packet Received");

//            // Basic latitude longitude decode example
//            int latRaw = BitConverter.ToInt32(data.Skip(11).Take(4).Reverse().ToArray());
//            int lonRaw = BitConverter.ToInt32(data.Skip(15).Take(4).Reverse().ToArray());

//            double latitude = latRaw / 1800000.0;
//            double longitude = lonRaw / 1800000.0;

//            Console.WriteLine($"Location → Lat: {latitude}  Lon: {longitude}");
//        }
//    }
//}



namespace GPS_Listener.Parser
{
    public class PacketParser
    {
        public void Parse(byte[] data, TcpClient client)
        {
            if (data.Length < 5)
            {
                Console.WriteLine("Invalid Packet");
                return;
            }

            byte protocol = data[3];

            Console.WriteLine($"Protocol: 0x{protocol:X2}");

            switch (protocol)
            {
                case 0x01:
                    LoginHandler.Handle(data, client);
                    break;

                case 0x13:
                    HeartbeatHandler.Handle(data, client);
                    break;

                case 0x12:
                    LocationHandler.Handle(data, client);
                    break;

                default:
                    Console.WriteLine("Unknown Packet");
                    break;
            }
        }
    }
}