using System;
using System.Net;
using System.Net.Sockets;
using System.Collections.Concurrent;
using System.Linq;
using System.Threading.Tasks;
using GPS_Listener.Parser;
using GPS_Listener.Models;

namespace GPS_Listener.Listener
{
    public class TcpGpsListener
    {
        private TcpListener listener;

        private static ConcurrentDictionary<string, DeviceSession> sessions =
            new ConcurrentDictionary<string, DeviceSession>();

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
            try
            {
                var stream = client.GetStream();
                byte[] buffer = new byte[1024];

                while (client.Connected)
                {
                    int bytes = await stream.ReadAsync(buffer, 0, buffer.Length);

                    if (bytes == 0)
                    {
                        Console.WriteLine("Device Disconnected");
                        break;
                    }

                    byte[] packet = buffer.Take(bytes).ToArray();

                    Console.WriteLine("\nPacket Received:");
                    Console.WriteLine(BitConverter.ToString(packet));

                    PacketParser.Parse(packet, client);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Connection Error: {ex.Message}");
            }
            finally
            {
                client.Close();
                Console.WriteLine("Connection Closed\n");
            }
        }
    }
}