using Outbox.Shared;
using Outbox.Shared.Strategy.Abstractions;

namespace StoreFront.Strategy.Implementations
{
    public class StoreFrontInboxMessageProcessor : IInboxMessageProcessor
    {
        public async Task<bool> ProcessMessageAsync(EventMessage message)
        {
            Console.WriteLine($"{DateTime.Now}: Store Front Inbox: <{message.EventName}, {message.Payload}>");
            await Task.CompletedTask;
            return true;
        }
    }
}
