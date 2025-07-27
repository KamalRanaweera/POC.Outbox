using Outbox.Shared;
using Outbox.Shared.Strategy.Abstractions;

namespace ShipmentProcessor.Strategy.Implementations
{
    public class ShipmentProcessorInboxMessageProcessor : IInboxMessageProcessor
    {
        public async Task<bool> ProcessMessageAsync(EventMessage message)
        {
            Console.WriteLine($"Shipment Processor Inbox: {message.EventName} > {message.Payload}");
            await Task.CompletedTask;
            return true;
        }
    }
}
