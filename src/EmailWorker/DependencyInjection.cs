using Ardalis.GuardClauses;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SmartFridgeManagerWorkers.EmailWorker.Common.Services;
using SmartFridgeManagerWorkers.EmailWorker.Common.Settings;

namespace SmartFridgeManagerWorkers.EmailWorker;

public static class DependencyInjection
{
    public static IServiceCollection AddEmailWorker(this IServiceCollection services, IConfiguration configuration)
    {
        string hostname = Guard.Against.NullOrEmpty(configuration["RABBITMQ_HOSTNAME"], "RABBITMQ_HOSTNAME");
        string port = Guard.Against.NullOrEmpty(configuration["RABBITMQ_PORT"], "RABBITMQ_PORT");
        string user = Guard.Against.NullOrEmpty(configuration["RABBITMQ_USER"], "RABBITMQ_USER");
        string password = Guard.Against.NullOrEmpty(configuration["RABBITMQ_PASS"], "RABBITMQ_PASS");

        RabbitMqSettings rabbitMqSettings = new()
        {
            HostName = hostname, Port = Convert.ToInt32(port), UserName = user, Password = password
        };

        services.AddSingleton(rabbitMqSettings);
        services.AddSingleton<IRabbitMqService, RabbitMqService>();

        services.AddHostedService<Worker>();
        return services;
    }
}
