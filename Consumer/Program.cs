
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using Shared.Configurations;
using System.Text;

var configuration = new ConfigurationBuilder()
    .SetBasePath(AppContext.BaseDirectory)
    .AddJsonFile("appsettings.json")
    .Build();

var rabbitMqOptions = configuration
    .GetSection(RabbitMQOptions.SectionName)
    .Get<RabbitMQOptions>()!;

var factory = new ConnectionFactory
{
    HostName = rabbitMqOptions.Server,
    UserName = rabbitMqOptions.UserName,
    Password = rabbitMqOptions.Password,
};

using var connection = await factory.CreateConnectionAsync();

using var channel = await connection.CreateChannelAsync();


await channel.QueueDeclareAsync(queue: rabbitMqOptions.QueueName, durable: true, exclusive: false, autoDelete: false);

var consumer = new AsyncEventingBasicConsumer(channel);

consumer.ReceivedAsync += async (sender, args) =>
{
    var body = args.Body.ToArray();

    var message = Encoding.UTF8.GetString(body);

    Console.WriteLine($"Received: {message}");

    await channel.BasicAckAsync(deliveryTag: args.DeliveryTag, multiple: false);
};

await channel.BasicConsumeAsync(queue: rabbitMqOptions.QueueName, autoAck: true, consumer: consumer);

Console.WriteLine("Waiting for message...");