using System;
using System.Net.Sockets;
using GPS_Listener.Models;

public static class LoginHandler
{
    public static void Handle(byte[] data, TcpClient client)
    {
        // IMEI extract
        string imei = BitConverter.ToString(data, 4, 8).Replace("-", "");

        Console.WriteLine($"Device Login: {imei}");

        // Create session
        var session = new DeviceSession
        {
            Imei = imei,
            Client = client,
            IsLoggedIn = true,
            LastHeartbeat = DateTime.UtcNow,
            LastLocation = DateTime.UtcNow
        };

        // Store in dictionary
        SessionManager.Sessions[imei] = session;

        Console.WriteLine($"Session Created for: {imei}");

        // Send ACK
        SendLoginAck(client);
    }

    private static void SendLoginAck(TcpClient client)
    {
        try
        {
            byte[] response = new byte[]
            {
                0x78,0x78,0x05,0x01,0x00,0x01,0xD9,0xDC,0x0D,0x0A
            };

            client.GetStream().Write(response, 0, response.Length);

            Console.WriteLine("Login ACK Sent");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"ACK Error: {ex.Message}");
        }
    }
}