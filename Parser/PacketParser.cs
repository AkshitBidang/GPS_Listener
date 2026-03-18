//using System;
//using System.Net.Sockets;

//namespace GPS_Listener.Parser
//{
//    public class PacketParser
//    {
//        public void Parse(byte[] data, TcpClient client)
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
//                    LoginHandler.Handle(data, client);
//                    break;

//                case 0x13:
//                    HeartbeatHandler.Handle(data, client);
//                    break;

//                case 0x12:
//                    LocationHandler.Handle(data, client);
//                    break;

//                default:
//                    Console.WriteLine("Unknown Packet");
//                    break;
//            }
//        }
//    }
//}



using System;
using System.Net.Sockets;

namespace GPS_Listener.Parser
{
    public class PacketParser
    {
        public void Parse(byte[] data, TcpClient client)
        {
            try
            {
                if (data == null || data.Length < 5)
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
                        Console.WriteLine($"Unknown Packet: 0x{protocol:X2}");
                        break;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Parser Error: {ex.Message}");
            }
        }
    }
}