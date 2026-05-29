namespace LayeredArchitecture_Task2_Catalog_Service.MessageQueue.Interfaces;

public interface IMessagePublisher : IAsyncDisposable
{
    Task PublishAsync(string exchange, string routingKey, object message, CancellationToken cancellationToken = default);
}