using Outbox.Shared;
using Outbox.Shared.Strategy.Abstractions;

namespace StoreFront.Strategy.Implementations
{
    public class StoreFrontInboxMessageProcessor : IInboxMessageProcessor
    {
        public async Task<bool> ProcessMessageAsync(EventMessage message)
        {
            Console.WriteLine("StoreFrontInboxMessageProcessor.ProcessMessageAsync");
            await Task.CompletedTask;
            return false;
        }
    }
}
