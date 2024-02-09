using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;

namespace Micro.Async.User.Persistance
{
    public class MessageBroker : IMessageBroker
    {
        private readonly ILogger<MessageBroker> logger;

        public MessageBroker(ILogger<MessageBroker> logger)
        {
            this.logger = logger;
        }
        public string Consume()
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

            var consumer = new EventingBasicConsumer(channel);
            var result = "";
            consumer.Received += (sender, args) =>
            {
                var body = args.Body.ToArray();
                result = Encoding.UTF8.GetString(body);
                logger.LogInformation($"{result}");
            };
            channel.BasicConsume(queue: "grades", autoAck: true, consumer: consumer);

            return result;
        }
    }
}
