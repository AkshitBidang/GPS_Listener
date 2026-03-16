using System.Net.Sockets;

public static class LocationHandler
{
    public static void Handle(byte[] data, TcpClient client)
    {
        int latRaw = BitConverter.ToInt32(data.Skip(11).Take(4).Reverse().ToArray());
        int lonRaw = BitConverter.ToInt32(data.Skip(15).Take(4).Reverse().ToArray());

        double latitude = latRaw / 1800000.0;
        double longitude = lonRaw / 1800000.0;

        Console.WriteLine($"Location: {latitude}, {longitude}");
    }
}