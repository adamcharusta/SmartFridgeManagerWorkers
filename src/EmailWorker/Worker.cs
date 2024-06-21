using Microsoft.Extensions.Hosting;

namespace SmartFridgeManagerWorkers.EmailWorker;

public class Worker : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            Console.WriteLine($"EmailWorker running at: {DateTime.Now}");

            await Task.Delay(1000, stoppingToken);
        }
    }
}
