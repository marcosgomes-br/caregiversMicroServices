using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;

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
        Console.WriteLine($"----> Recebeu a mensagem do RabbitMQ || Latitude: {geoLocation.Latitude} Longitude: {geoLocation.Longitude}");
        using var httpClient = new HttpClient()
        {
            BaseAddress = new Uri("https://localhost:7265"),
        };
        var url = $"/suppliers?latitude={geoLocation.Latitude.ToString().Replace(",",".")}&longitude={geoLocation.Longitude.ToString().Replace(",", ".")}";
        var response = httpClient.GetAsync(url).GetAwaiter().GetResult();
        if (response.IsSuccessStatusCode)
        {
            var suppliers = JsonSerializer.Deserialize<List<dynamic>>(response.Content.ReadAsStreamAsync().GetAwaiter().GetResult());
            Console.WriteLine($"---> Cuidadores disponíveis: {suppliers?.Count ?? 0}");
        }
    };
    channel.BasicConsume(queue: "caregiversRequestSuppliers",
                         autoAck: true,
                         consumer: consumer);

    Console.WriteLine(" Press [enter] to exit.");
    Console.ReadLine();
}

record GeoLocation
{
    [JsonPropertyName("latitude")]
    public double Latitude { get; set; }
    [JsonPropertyName("longitude")]
    public double Longitude { get; set; }
}