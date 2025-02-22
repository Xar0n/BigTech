namespace BigTech.Producer.Interfaces;
public interface IMessageProducer
{
    Task SendMessage<T>(T message, string routingKey, string exchange);
}
