using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Shared.Configurations;
using System.Text;
using System.Text.Json;

namespace Producer.Services;

public class MessageProducer(IOptions<RabbitMQOptions> rabbitMQOptions) : IMessageProducer
{
    private readonly RabbitMQOptions _rabbitMQOptions = rabbitMQOptions.Value;

    public async Task SendingMessageAsync<T>(T message, CancellationToken cancellationToken = default)
    {
        var factory = new ConnectionFactory()
        {
            HostName = _rabbitMQOptions.Server,
            UserName = _rabbitMQOptions.UserName,
            Password = _rabbitMQOptions.Password,
        };

        using var connection = await factory.CreateConnectionAsync(cancellationToken);

        using var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            _rabbitMQOptions.QueueName, durable: true, exclusive: false, autoDelete: false, cancellationToken: cancellationToken);

        var body = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(message));

        await channel.BasicPublishAsync(exchange: string.Empty, routingKey: _rabbitMQOptions.QueueName, body: body, cancellationToken: cancellationToken);
    }
}
