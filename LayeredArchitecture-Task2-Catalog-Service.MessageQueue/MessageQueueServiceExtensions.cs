using LayeredArchitecture_Task2_Catalog_Service.MessageQueue.Implementation;
using LayeredArchitecture_Task2_Catalog_Service.MessageQueue.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace LayeredArchitecture_Task2_Catalog_Service.MessageQueue;

public static class MessageQueueServiceExtensions
{
    public static IServiceCollection AddMessageQueue(this IServiceCollection services)
    {
        services.AddSingleton<IMessagePublisher>(sp =>
        {
            var options = sp.GetRequiredService<IOptions<RabbitMQOptions>>();
            var settings = options.Value;
            var publisher = RabbitMQPublisher.CreateAsync(settings.HostName, settings.Port).GetAwaiter().GetResult();

            var productUpdated = settings.ProductUpdated;
            publisher.EnsureTopologyAsync(productUpdated.Exchange, productUpdated.Queue, productUpdated.RoutingKey)
                .GetAwaiter().GetResult();

            return publisher;
        });

        return services;
    }
}