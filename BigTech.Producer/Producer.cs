using BigTech.Producer.Interfaces;
using Newtonsoft.Json;
using RabbitMQ.Client;
using System.Text;

namespace BigTech.Producer;
public class Producer : IMessageProducer
{
    public async Task SendMessage<T>(T message, string routingKey, string exchange)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5670,
            Password = "password",
            UserName = "user",
        };
        var connection = await factory.CreateConnectionAsync();
        using var channel = await connection.CreateChannelAsync();

        var json = JsonConvert.SerializeObject(message,
            Formatting.Indented, new JsonSerializerSettings()
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore,
            });
        var body = Encoding.UTF8.GetBytes(json);

        await channel.BasicPublishAsync(exchange, routingKey, body);
    }
}
