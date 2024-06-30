using Domain.Common;

namespace SmartFridgeManagerWorkers.EmailWorker.Common.Services;

public interface IRabbitMqService
{
    void ReadAllMessages<T>(BaseQueue<T> queue, Action<T> action)
        where T : class;
}
