using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outbox.Shared.Strategy.Abstractions
{
    public interface IMessageBrokerAgent
    {
        Task<bool> Publish(string eventName, object payload);
    }
}
