using System.Net.Sockets;

public static class HeartbeatHandler
{
    public static void Handle(byte[] data, TcpClient client)
    {
        Console.WriteLine("Heartbeat received");

        byte[] response = new byte[]
        {
            0x78,0x78,0x05,0x13,0x00,0x01,0xD9,0xDC,0x0D,0x0A
        };

        client.GetStream().Write(response);
    }
}