using RabbitMQ.Client.Events;
using RabbitMQ.Client;
using Microsoft.AspNetCore.Connections;
using System.Text;
using Newtonsoft.Json;

namespace Micro.Sinhro.Gift.Persistance
{
    public class MessageBroker : IMessageBroker
    { 
            public void Publish<T>(T message)
            {
                var factory = new ConnectionFactory()
                {
                    HostName = "localhost",
                    Port = 5672,
                    UserName = "guest",
                    Password = "guest"
                };
                var connection = factory.CreateConnection();
                using var channel = connection.CreateModel();
                channel.QueueDeclare(queue: "grades", durable: false, exclusive: false,
                    autoDelete: false, arguments: null);
                var body = Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(message));
                channel.BasicPublish("", "grades", body: body);
            }
        
    }
}
