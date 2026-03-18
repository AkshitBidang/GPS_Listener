//using System;
//using System.Net;
//using System.Net.Sockets;
//using System.Linq;
//using System.Threading.Tasks;
//using System.Collections.Generic;
//using GPS_Listener.Parser;
//using GPS_Listener.Models;

//namespace GPS_Listener.Listener
//{
//    public class TcpGpsListener
//    {
//        private TcpListener listener;

//        public TcpGpsListener(int port)
//        {
//            listener = new TcpListener(IPAddress.Any, port);
//        }

//        public async Task StartAsync()
//        {
//            listener.Start();

//            Console.WriteLine("GPS Listener started...");

//            while (true)
//            {
//                TcpClient client = await listener.AcceptTcpClientAsync();

//                Console.WriteLine($"Device Connected : {client.Client.RemoteEndPoint}");

//                _ = Task.Run(() => HandleClient(client));
//            }
//        }

//        private async Task HandleClient(TcpClient client)
//        {
//            var parser = new PacketParser(); // 🔥 per-client parser

//            try
//            {
//                var stream = client.GetStream();
//                byte[] buffer = new byte[1024];

//                List<byte> packetBuffer = new List<byte>(); // 🔥 for fragmentation

//                while (client.Connected)
//                {
//                    int bytes = await stream.ReadAsync(buffer, 0, buffer.Length);

//                    if (bytes == 0)
//                    {
//                        Console.WriteLine("Device Disconnected (No Data)");
//                        break;
//                    }

//                    // Add incoming data to buffer
//                    packetBuffer.AddRange(buffer.Take(bytes));

//                    // 🔥 Process complete packets
//                    while (packetBuffer.Count >= 5)
//                    {
//                        // Check start bytes 0x78 0x78
//                        if (packetBuffer[0] != 0x78 || packetBuffer[1] != 0x78)
//                        {
//                            packetBuffer.RemoveAt(0);
//                            continue;
//                        }

//                        int length = packetBuffer[2];

//                        int fullPacketLength = length + 5;
//                        // 2(start) + 1(length) + content + 2(stop)

//                        if (packetBuffer.Count < fullPacketLength)
//                            break; // wait for more data

//                        byte[] packet = packetBuffer.Take(fullPacketLength).ToArray();

//                        Console.WriteLine("\nPacket Received:");
//                        Console.WriteLine(BitConverter.ToString(packet));

//                        // Parse packet
//                        parser.Parse(packet, client);

//                        // Remove processed packet
//                        packetBuffer.RemoveRange(0, fullPacketLength);
//                    }
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Connection Error: {ex.Message}");
//            }
//            finally
//            {
//                RemoveSession(client);

//                client.Close();
//                Console.WriteLine("Connection Closed\n");
//            }
//        }

//        // 🔥 Session cleanup on disconnect
//        private void RemoveSession(TcpClient client)
//        {
//            var session = SessionManager.Sessions.Values
//                .FirstOrDefault(x => x.Client == client);

//            if (session != null)
//            {
//                SessionManager.Sessions.TryRemove(session.Imei, out _);

//                Console.WriteLine($"Device Removed: {session.Imei}");
//                Console.WriteLine($"Active Devices: {SessionManager.Sessions.Count}");
//            }
//            else
//            {
//                Console.WriteLine("Disconnected unknown device");
//            }
//        }
//    }
//}



using System;
using System.Net;
using System.Net.Sockets;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using GPS_Listener.Parser;
using GPS_Listener.Models;

namespace GPS_Listener.Listener
{
    public class TcpGpsListener
    {
        private TcpListener listener;

        public TcpGpsListener(int port)
        {
            listener = new TcpListener(IPAddress.Any, port);
        }

        public async Task StartAsync()
        {
            listener.Start();

            Console.WriteLine("GPS Listener started...");

            while (true)
            {
                TcpClient client = await listener.AcceptTcpClientAsync();

                Console.WriteLine($"Device Connected : {client.Client.RemoteEndPoint}");

                _ = Task.Run(() => HandleClient(client));
            }
        }

        private async Task HandleClient(TcpClient client)
        {
            var parser = new PacketParser();

            try
            {
                var stream = client.GetStream();
                byte[] buffer = new byte[1024];

                using var ms = new MemoryStream();

                while (client.Connected)
                {
                    int bytesRead = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytesRead == 0)
                    {
                        Console.WriteLine("Device Disconnected (No Data)");
                        break;
                    }

                    // Append data
                    ms.Write(buffer, 0, bytesRead);

                    while (true)
                    {
                        byte[] data = ms.ToArray();

                        if (data.Length < 5)
                            break;

                        // Validate start bytes
                        if (data[0] != 0x78 || data[1] != 0x78)
                        {
                            Console.WriteLine("Invalid start bytes, clearing buffer");
                            ms.SetLength(0);
                            break;
                        }

                        int length = data[2];
                        int fullPacketLength = length + 5;

                        if (data.Length < fullPacketLength)
                            break;

                        // Extract packet
                        byte[] packet = data.Take(fullPacketLength).ToArray();

                        Console.WriteLine("\nPacket Received:");
                        Console.WriteLine(BitConverter.ToString(packet));

                        // Parse
                        parser.Parse(packet, client);

                        // Remove processed packet
                        byte[] remaining = data.Skip(fullPacketLength).ToArray();

                        ms.SetLength(0);
                        ms.Write(remaining, 0, remaining.Length);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection Error: {ex.Message}");
            }
            finally
            {
                RemoveSession(client);

                client.Close();
                Console.WriteLine("Connection Closed\n");
            }
        }

        // 🔥 OPTIMIZED REMOVE SESSION (O(1))
        private void RemoveSession(TcpClient client)
        {
            // direct lookup (no LINQ)
            if (SessionManager.ClientMap.TryRemove(client, out string imei))
            {
                SessionManager.Sessions.TryRemove(imei, out _);

                Console.WriteLine($"Device Removed: {imei}");
                Console.WriteLine($"Active Devices: {SessionManager.Sessions.Count}");
            }
            else
            {
                Console.WriteLine("Disconnected unknown device");
            }
        }
    }
}