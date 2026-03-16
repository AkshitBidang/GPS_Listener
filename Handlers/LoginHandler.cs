using System.Net.Sockets;

public static class LoginHandler
{
    public static void Handle(byte[] data, TcpClient client)
    {
        string imei = BitConverter.ToString(data, 4, 8).Replace("-", "");

        Console.WriteLine($"Device Login: {imei}");

        SendLoginAck(client);
    }

    private static void SendLoginAck(TcpClient client)
    {
        byte[] response = new byte[]
        {
            0x78,0x78,0x05,0x01,0x00,0x01,0xD9,0xDC,0x0D,0x0A
        };

        client.GetStream().Write(response);
    }
}