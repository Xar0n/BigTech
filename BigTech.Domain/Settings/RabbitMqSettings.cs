namespace BigTech.Domain.Settings;
public class RabbitMqSettings
{
    public const string DefaultSection = "RabbitMq";

    public required string HostName { get; set; }

    public required int Port { get; set; }

    public required string Password { get; set; }

    public required string UserName { get; set; }

    public required string QueueName { get; set; }
    
    public required string RoutingKey { get; set; }

    public required string ExchangeName { get; set; }
}
