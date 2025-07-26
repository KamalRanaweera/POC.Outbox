
namespace ShipmentProcessor.Models
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public Guid OutboxMessageId { get; set; }

    }
}
