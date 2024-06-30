namespace SmartFridgeManagerWorkers.EmailWorker.Common.Settings;

public class RabbitMqSettings
{
    public int Port { get; init; }
    public required string HostName { get; init; }
    public required string UserName { get; init; }
    public required string Password { get; init; }
}
