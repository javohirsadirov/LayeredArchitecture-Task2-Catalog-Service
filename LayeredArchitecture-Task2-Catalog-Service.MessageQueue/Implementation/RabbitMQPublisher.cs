using System.Text;
using System.Text.Json;

using CatalogService.MessageQueue.Interfaces;

using RabbitMQ.Client;

namespace CatalogService.MessageQueue.Implementation;

internal class RabbitMQPublisher : IMessagePublisher, IAsyncDisposable
{
    private readonly IConnection connection;
    private readonly IChannel channel;

    private RabbitMQPublisher(IConnection connection, IChannel channel)
    {
        this.connection = connection;
        this.channel = channel;
    }

    public static async Task<RabbitMQPublisher> CreateAsync(string hostName, int port, CancellationToken cancellationToken = default)
    {
        var factory = new ConnectionFactory
        {
            HostName = hostName,
            Port = port
        };

        var connection = await factory.CreateConnectionAsync(cancellationToken);
        var channel = await connection.CreateChannelAsync(cancellationToken: cancellationToken);

        return new RabbitMQPublisher(connection, channel);
    }

    internal async Task EnsureTopologyAsync(string exchange, string queue, string routingKey, CancellationToken cancellationToken = default)
    {
        await channel.ExchangeDeclareAsync(
            exchange: exchange,
            type: ExchangeType.Direct,
            durable: true,
            cancellationToken: cancellationToken);

        await channel.QueueDeclareAsync(
            queue: queue,
            durable: true,
            exclusive: false,
            autoDelete: false,
            cancellationToken: cancellationToken);

        await channel.QueueBindAsync(
            queue: queue,
            exchange: exchange,
            routingKey: routingKey,
            cancellationToken: cancellationToken);
    }

    public async Task PublishAsync(string exchange, string routingKey, object message, CancellationToken cancellationToken = default)
    {
        var json = JsonSerializer.Serialize(message);
        var body = Encoding.UTF8.GetBytes(json);

        var properties = new BasicProperties { Persistent = true };

        await channel.BasicPublishAsync(
            exchange: exchange,
            routingKey: routingKey,
            mandatory: false,
            basicProperties: properties,
            body: body,
            cancellationToken: cancellationToken);
    }

    public async ValueTask DisposeAsync()
    {
        await channel.CloseAsync();
        await connection.CloseAsync();
        channel.Dispose();
        connection.Dispose();
        GC.SuppressFinalize(this);
    }
}
