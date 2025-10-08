using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.IntegrationEvents.MessageBroker.Kafka.Interfaces
{
    public interface IEventHandler<T>
    {
        Task HandleAsync(T @event);
    }
}
