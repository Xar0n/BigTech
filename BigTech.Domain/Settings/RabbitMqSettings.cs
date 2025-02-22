namespace BigTech.Domain.Settings;
public class RabbitMqSettings
{
    public const string DefaultSection = "RabbitMq";

    public required string QueueName { get; set; }
    
    public required string RoutingKey { get; set; }

    public required string ExchangeName { get; set; }
}
