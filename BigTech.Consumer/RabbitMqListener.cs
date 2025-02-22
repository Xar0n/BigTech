using BigTech.Domain.Settings;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Diagnostics;
using System.Text;

namespace BigTech.Consumer;
public class RabbitMqListener : BackgroundService
{
    private IConnection? _connection;
    private IChannel? _channel;
    private readonly IOptions<RabbitMqSettings> _rabbitMqSettings;

    public RabbitMqListener(IOptions<RabbitMqSettings> rabbitMqSettings)
    {
        _rabbitMqSettings = rabbitMqSettings;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var factory = new ConnectionFactory()
        {
            HostName = "localhost",
            Port = 5670,
            Password = "password",
            UserName = "user",
        };

        _connection = await factory.CreateConnectionAsync();
        _channel = await _connection.CreateChannelAsync();

        await _channel.QueueDeclareAsync(
            _rabbitMqSettings.Value.QueueName,
            durable: true,
            exclusive: false,
            autoDelete: false,
            arguments: null);


        var consumer = new AsyncEventingBasicConsumer(_channel);
        consumer.ReceivedAsync += async (obj, basicDeliver) =>
        {
            var content = Encoding.UTF8.GetString(basicDeliver.Body.ToArray());
            Debug.WriteLine($"Получено сообщение: {content}");

            await _channel.BasicAckAsync(basicDeliver.DeliveryTag, false);
        };
        await _channel.BasicConsumeAsync(_rabbitMqSettings.Value.QueueName, 
            false, consumer);
    }

    public override void Dispose()
    {
        _channel?.Dispose();
        _connection?.Dispose();
        base.Dispose();
    }
}
