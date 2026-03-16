using GPS_Listener.Listener;

class Program
{
    static async Task Main(string[] args)
    {
        TcpGpsListener server = new TcpGpsListener(9505);

        await server.StartAsync();
    }
}