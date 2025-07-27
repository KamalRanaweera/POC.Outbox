using ShipmentProcessor.Models;

namespace ShipmentProcessor.Interfaces
{
    public interface IShipmentService
    {
        public Task<List<Shipment>> ListPendingShipments();
        public Task<Shipment?> GetShipment(int orderId);
        public Task<bool> CompleteShipment(Guid shipmentId);

    }
}
