using System;
using System.Net.Sockets;
using System.Linq;
using GPS_Listener.Models;

public static class HeartbeatHandler
{
    public static void Handle(byte[] data, TcpClient client)
    {
        // Find session by client
        var session = SessionManager.Sessions.Values
            .FirstOrDefault(x => x.Client == client);

        if (session != null)
        {
            session.LastHeartbeat = DateTime.UtcNow;

            Console.WriteLine($"Heartbeat received from: {session.Imei}");
        }
        else
        {
            Console.WriteLine("Heartbeat received (Unknown Device)");
        }

        SendHeartbeatAck(client);
    }

    private static void SendHeartbeatAck(TcpClient client)
    {
        try
        {
            byte[] response = new byte[]
            {
                0x78,0x78,0x05,0x13,0x00,0x01,0xD9,0xDC,0x0D,0x0A
            };

            client.GetStream().Write(response, 0, response.Length);

            Console.WriteLine("Heartbeat ACK Sent");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Heartbeat ACK Error: {ex.Message}");
        }
    }
}