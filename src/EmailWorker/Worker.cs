using Domain.Queues;
using Microsoft.Extensions.Hosting;
using Newtonsoft.Json;
using SmartFridgeManagerWorkers.EmailWorker.Common.Services;

namespace SmartFridgeManagerWorkers.EmailWorker;

public class Worker(IRabbitMqService rabbitMqService) : BackgroundService
{
    private void HandleEmailMessage(Email msg)
    {
        Console.WriteLine(DateTime.Now + ": " + JsonConvert.SerializeObject(msg));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        while (!stoppingToken.IsCancellationRequested)
        {
            rabbitMqService.ReadAllMessages(new EmailQueue(), HandleEmailMessage);

            await Task.Delay(1000, stoppingToken);
        }
    }
}
