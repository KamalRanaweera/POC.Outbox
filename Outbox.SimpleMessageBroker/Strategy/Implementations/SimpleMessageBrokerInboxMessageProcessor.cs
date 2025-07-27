using Outbox.Shared;
using Outbox.Shared.Strategy.Abstractions;

namespace Outbox.SimpleMessageBroker.Strategy.Implementations
{
    public class SimpleMessageBrokerInboxMessageProcessor : IInboxMessageProcessor
    {
        public async Task<bool> ProcessMessageAsync(EventMessage message)
        {
            Console.WriteLine($"Message Broker Inbox: {message.EventName} > {message.Payload}");
            await Task.CompletedTask;
            return true;
        }
    }
}
