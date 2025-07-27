using Outbox.Shared;
using Outbox.Shared.Strategy.Abstractions;

namespace Outbox.SimpleMessageBroker.Strategy.Implementations
{
    public class SimpleMessageBrokerInboxMessageProcessor : IInboxMessageProcessor
    {
        public async Task<bool> ProcessMessageAsync(EventMessage message)
        {
            Console.WriteLine("StoreFrontInboxMessageProcessor.ProcessMessageAsync");
            await Task.CompletedTask;
            return false;
        }
    }
}
