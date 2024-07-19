using Caregivers.Consumers.Api.Domain;
using Microsoft.AspNetCore.Connections;
using System.Text.Json;
using System.Text;
using RabbitMQ.Client;

namespace Caregivers.Consumers.Api.Services
{
    public class ConsumerService
    {
        public void SendMessage(Consumer consumer)
        {
            var factory = new ConnectionFactory()
            {
                HostName = "localhost",
                UserName = "admin",
                Password = "admiN@1234",
                Port = 5672
            };
            using (var connection = factory.CreateConnection())
            using (var channel = connection.CreateModel())
            {
                channel.QueueDeclare(queue: "caregiversRequestSuppliers",
                                     durable: false,
                                     exclusive: false,
                                     autoDelete: false,
                                     arguments: null);

                string json = JsonSerializer.Serialize(new { latitude = consumer.Latitude, longitude = consumer.Longitude });
                var body = Encoding.UTF8.GetBytes(json);

                channel.BasicPublish(exchange: "",
                                     routingKey: "caregiversRequestSuppliers",
                                     basicProperties: null,
                                     body: body);
            }
        }
    }
}
