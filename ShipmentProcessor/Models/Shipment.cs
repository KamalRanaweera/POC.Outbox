
using System.Text.Json.Serialization;

namespace ShipmentProcessor.Models
{
    public class Shipment
    {
        public Guid Id { get; set; }
        public int OrderId { get; set; }
        public ShipmentStatus ShipmentStatus { get; set; } = ShipmentStatus.Open;
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime? UpdatedAt { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ShipmentStatus { Open = 0, InProgress, Complete}
}
