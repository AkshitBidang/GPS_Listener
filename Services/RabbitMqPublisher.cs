//using Microsoft.EntityFrameworkCore.Metadata;
//using RabbitMQ.Client;
//using System.Text;

//public class RabbitMqPublisher
//{
//    private readonly IConnection connection;
//    private readonly IModel channel;

//    public RabbitMqPublisher()
//    {
//        var factory = new ConnectionFactory()
//        {
//            HostName = "localhost"
//        };

//        connection = factory.CreateConnection();
//        channel = connection.CreateModel();

//        channel.QueueDeclare(
//            queue: "gps_packets",
//            durable: true,
//            exclusive: false,
//            autoDelete: false,
//            arguments: null);
//    }

//    public void Publish(byte[] packet)
//    {
//        var properties = channel.CreateBasicProperties();
//        properties.Persistent = true;

//        channel.BasicPublish(
//            exchange: "",
//            routingKey: "gps_packets",
//            basicProperties: properties,
//            body: packet);
//    }
//}



using RabbitMQ.Client;
using System.Text;

namespace GPS_Listener.Services
{
    public class RabbitMqPublisher
    {
        private readonly IConnection connection;

        // 🔥 FULL QUALIFIED NAME (fix)
        private readonly RabbitMQ.Client.IModel channel;

        public RabbitMqPublisher()
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost"
            };

            connection = factory.CreateConnection();

            // 🔥 FIX HERE ALSO
            channel = connection.CreateModel();

            channel.QueueDeclare(
                queue: "gps_packets",
                durable: true,
                exclusive: false,
                autoDelete: false,
                arguments: null);
        }

        public void Publish(byte[] packet)
        {
            var properties = channel.CreateBasicProperties();
            properties.Persistent = true;

            channel.BasicPublish(
                exchange: "",
                routingKey: "gps_packets",
                basicProperties: properties,
                body: packet);
        }
    }
}