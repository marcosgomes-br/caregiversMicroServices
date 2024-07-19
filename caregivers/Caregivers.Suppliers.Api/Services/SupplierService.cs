using Caregivers.Suppliers.Api.Data;
using Caregivers.Suppliers.Api.Repositories;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;

namespace Caregivers.Suppliers.Api.Services
{
    public class SupplierService(SupplierRepository _repository)
    {
        private record GeoLocation
        {
            public double Latitude { get; set; }
            public double Longitude { get; set; }
        }

        public void ReceiveMessage()
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

                var consumer = new EventingBasicConsumer(channel);
                consumer.Received += (model, ea) =>
                {
                    var body = ea.Body.ToArray();
                    var json = Encoding.UTF8.GetString(body);

                    var geoLocation = JsonSerializer.Deserialize<GeoLocation>(json);
                    if (geoLocation == null)
                        throw new Exception("Falha no recebimento da mensagem");
                    FindSuppliers(geoLocation.Longitude, geoLocation.Latitude);
                };
                channel.BasicConsume(queue: "caregiversRequestSuppliers",
                                     autoAck: true,
                                     consumer: consumer);
            }
        }

        private void FindSuppliers(double longitude, double latitude)
        {
            var suppliers = _repository.Find(longitude, latitude);
            Console.WriteLine($"---> Cuidadores disponíveis: {suppliers.Count}");
        }
    }
}
