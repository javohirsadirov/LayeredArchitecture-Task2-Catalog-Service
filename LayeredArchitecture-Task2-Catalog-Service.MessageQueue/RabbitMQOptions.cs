namespace LayeredArchitecture_Task2_Catalog_Service.MessageQueue;

public class RabbitMQOptions
{
    public const string SectionName = "RabbitMQOptions";

    public string HostName { get; set; } = "localhost";
    public int Port { get; set; } = 5672;
    public ProductUpdatedSettings ProductUpdated { get; set; } = new();
}

public class ProductUpdatedSettings
{
    public string Exchange { get; set; } = string.Empty;
    public string Queue { get; set; } = string.Empty;
    public string RoutingKey { get; set; } = string.Empty;
}
