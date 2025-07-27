using ShipmentProcessor.Models;

namespace ShipmentProcessor.Dtos
{
    public class ShipmentDto
    {
        public Guid Id { get; set; }
        public int OrderId { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; } = ShipmentStatus.Open;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }
}
