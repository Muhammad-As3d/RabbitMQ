namespace Producer.Services;

public interface IMessageProducer
{
    Task SendingMessageAsync<T>(T message, CancellationToken cancellationToken = default);
}
