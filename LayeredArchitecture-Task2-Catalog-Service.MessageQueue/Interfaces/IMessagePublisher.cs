namespace CatalogService.MessageQueue.Interfaces;

public interface IMessagePublisher : IAsyncDisposable
{
    Task PublishAsync(string exchange, string routingKey, object message, CancellationToken cancellationToken = default);
}
