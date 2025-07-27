using Microsoft.EntityFrameworkCore;
using Outbox.Shared;
using Outbox.Shared.Strategy.Abstractions;
using ShipmentProcessor.Models;
using System.Data.SqlTypes;

namespace ShipmentProcessor.Strategy.Implementations
{
    public class ShipmentProcessorInboxMessageProcessor : IInboxMessageProcessor
    {
        private readonly ShipmentDbContext _db;
        private readonly ILogger<ShipmentProcessorInboxMessageProcessor> _logger;

        public ShipmentProcessorInboxMessageProcessor(ShipmentDbContext db, ILogger<ShipmentProcessorInboxMessageProcessor> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<bool> ProcessMessageAsync(EventMessage message)
        {
            try
            {
                if (!int.TryParse(message.Payload, out int orderId))
                {
                    _logger.LogWarning($"ERROR: EventMessage {message.Id}: Payload should be an integer, representing the order ID.");
                    return false;
                }

                var shipment = await _db.Shipments.FirstOrDefaultAsync(s => s.OrderId == orderId);

                if (shipment == null)
                    return await CreateShipmentRecord(orderId); // No shipment is created yet, so create one.
            }
            catch (DbUpdateException)
            {
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\n{ex.StackTrace}");
            }
            return false;
        }

        private async Task<bool> CreateShipmentRecord(int orderId)
        {
            try
            {
                _db.Shipments.Add(new Shipment
                {
                    OrderId = orderId,
                    ShipmentStatus = ShipmentStatus.Open
                });
                await _db.SaveChangesAsync();

                Console.WriteLine($"{DateTime.Now}: Created shipment for order {orderId}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\n{ex.StackTrace}");
                return false;
            }
        }

        //private async Task<bool> ProcessShipment(Guid eventId, Guid shipmentId)
        //{
        //    try
        //    {
        //        using var tx = await _db.Database.BeginTransactionAsync();

        //        var shipment = (await _db.Shipments.FirstOrDefaultAsync(sh => sh.Id == shipmentId))!;
        //        if (shipment.ShipmentStatus == ShipmentStatus.Complete)
        //        {
        //            var requestEventMessage = (await _db.EventMessages.FindAsync(eventId))!;
        //            requestEventMessage.Processed = true;

        //            // Save the event message to be published by the message broker agent
        //            var eventMessage = new EventMessage
        //            {
        //                Id = Guid.NewGuid(),
        //                MessageType = MessageType.Outbox,
        //                EventName = EventNames.OrderComplete,
        //                Payload = shipment.OrderId.ToString(),
        //            };
        //            _db.EventMessages.Add(eventMessage);

        //            await _db.SaveChangesAsync();
        //        }
        //        await tx.CommitAsync();

        //        return true;
        //    }
        //    catch (Exception)
        //    {
        //        return false;
        //    }
        //}
    }
}
