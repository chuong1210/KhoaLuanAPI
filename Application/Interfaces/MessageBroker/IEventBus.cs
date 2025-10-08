using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Interfaces.MessageBroker
{
    public interface IEventBus
    {
        Task PublishAsync<T>(T @event, string topic = null) where T : class;
    }
}
