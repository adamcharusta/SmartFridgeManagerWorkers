using System.Text;
using Domain.Common;
using Newtonsoft.Json;
using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using SmartFridgeManagerWorkers.EmailWorker.Common.Settings;

namespace SmartFridgeManagerWorkers.EmailWorker.Common.Services;

public class RabbitMqService : IRabbitMqService
{
    private readonly IModel _channel;
    private readonly IConnection _connection;
    private readonly RabbitMqSettings _settings;

    public RabbitMqService(RabbitMqSettings settings)
    {
        _settings = settings;
        ConnectionFactory factory = new()
        {
            HostName = settings.HostName,
            Port = settings.Port,
            UserName = settings.UserName,
            Password = settings.Password
        };

        _connection = factory.CreateConnection();
        _channel = _connection.CreateModel();
    }

    public void ReadAllMessages<T>(BaseQueue<T> queue, Action<T> action)
        where T : class
    {
        _channel.QueueDeclare(
            queue.Queue,
            queue.Durable,
            queue.Exclusive,
            queue.AutoDelete,
            queue.Arguments
        );

        EventingBasicConsumer consumer = new(_channel);

        consumer.Received += (model, ea) =>
        {
            string message = Encoding.UTF8.GetString(ea.Body.ToArray());
            T? body = JsonConvert.DeserializeObject<T>(message);

            if (body != null)
            {
                action(body);
            }
        };

        _channel.BasicConsume(queue.Queue, queue.AutoAck, consumer);
    }

    ~RabbitMqService()
    {
        if (_channel.IsOpen)
        {
            _channel?.Close();
            _channel?.Dispose();
        }

        if (_connection.IsOpen)
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
