using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outbox.Shared.Strategy.Abstractions
{
    public interface IInboxMessageProcessor
    {
        Task<bool> ProcessMessageAsync(EventMessage message);
    }
}
