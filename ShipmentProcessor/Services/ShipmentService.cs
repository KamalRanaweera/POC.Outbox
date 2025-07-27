using Microsoft.EntityFrameworkCore;
using Outbox.Shared;
using ShipmentProcessor.Interfaces;
using ShipmentProcessor.Models;

namespace ShipmentProcessor.Services
{
    public class ShipmentService : IShipmentService
    {
        private readonly ShipmentDbContext _db;
        private readonly ILogger<ShipmentService> _logger;

        public ShipmentService(ShipmentDbContext db, ILogger<ShipmentService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<List<Shipment>> ListPendingShipments()
        {
            return await _db.Shipments.Where(shipment => shipment.ShipmentStatus != ShipmentStatus.Complete).ToListAsync();
        }

        public async Task<Shipment?> GetShipment(int orderId)
        {
            return await _db.Shipments.Where(shipment => shipment.OrderId == orderId).FirstOrDefaultAsync();
        }

        public async Task<bool> UpdateShipment(Guid shipmentId, ShipmentStatus status)
        {
            try
            {
                var tx = _db.Database.BeginTransaction();

                Shipment shipment = _db.Shipments.FirstOrDefault(sh => sh.Id == shipmentId)!;
                if (shipment == null)
                    return false;

                shipment.ShipmentStatus = status;

                // Save the event message to be published by the message broker agent
                var eventMessage = new EventMessage
                {
                    Id = Guid.NewGuid(),
                    MessageType = MessageType.Outbox,
                    EventName = status == ShipmentStatus.Complete ? EventNames.OrderComplete : EventNames.OrderInProgress,
                    Payload = shipment.OrderId.ToString(),
                };
                _db.EventMessages.Add(eventMessage);

                await _db.SaveChangesAsync();
                await tx.CommitAsync();

                Console.WriteLine($"Sending event {eventMessage.EventName} for order ID: {shipment.OrderId}");
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
