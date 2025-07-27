using Outbox.Shared;
using Outbox.Shared.Strategy.Abstractions;

namespace ShipmentProcessor.Strategy.Implementations
{
    public class ShipmentProcessorInboxMessageProcessor : IInboxMessageProcessor
    {
        public async Task<bool> ProcessMessageAsync(EventMessage message)
        {
            Console.WriteLine("ShipmentProcessorInboxMessageProcessor.ProcessMessageAsync");
            await Task.CompletedTask;
            return false;
        }
    }
}
