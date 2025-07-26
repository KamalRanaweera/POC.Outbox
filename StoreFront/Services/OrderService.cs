using Microsoft.EntityFrameworkCore;
using Outbox.Shared;
using Outbox.Shared.Dtos;
using StoreFront.Interfaces;
using StoreFront.Models;

namespace StoreFront.Services
{
    public class OrderService : IOrderService
    {
        private readonly StoreFrontDbContext _db;
        private readonly ILogger<OrderService> _logger;

        public OrderService(StoreFrontDbContext db, ILogger<OrderService> logger)
        {
            _db = db;
            _logger = logger;
        }

        public async Task<bool> PlaceOrder(Order order)
        {
            try
            {
                using var tx = await _db.Database.BeginTransactionAsync();

                await _db.Orders.AddAsync(order);
                foreach (var item in order.Items)
                {
                    var inventoryItem = _db.Inventory.FirstOrDefault(inventoryRecord => inventoryRecord.Id == item.InventoryRecordId);
                    if (inventoryItem == null)
                    {
                        _logger.LogWarning($"Item ID {item.InventoryRecordId}: not found in the enventory.");
                        return false;
                    }

                    if (inventoryItem.AvailableCount < item.Count)
                    {
                        _logger.LogWarning($"Item ID {item.InventoryRecordId}: insufficient quantity ({item.Count} required, {inventoryItem.AvailableCount} available).");
                        return false;
                    }

                    inventoryItem.AvailableCount -= item.Count;
                    inventoryItem.OnHoldCount += item.Count;
                }
                await _db.SaveChangesAsync();

                // Save the event message to be published by the message broker agent
                var eventMessage = new EventMessage
                {
                    Id = Guid.NewGuid(),
                    MessageType = MessageType.Outbox,
                    EventName = "OrderPlaced",
                    Payload = order.Id.ToString(),
                };
                _db.EventMessages.Add(eventMessage);

                await _db.SaveChangesAsync();
                await tx.CommitAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"{ex.Message}\n{ex.StackTrace}");
                return false;

            }

            //var itemz = await _db.Inventory.FirstOrDefaultAsync(x => x.Id == itemId);
            //if (item == null)
            //    throw new Exception("Item not found");

            //if (item.AvailableCount == 0)
            //    throw new Exception("Out of stock");

            //if (item.AvailableCount < amount)
            //    throw new Exception("Insufficient stock");

            //item.AvailableCount -= amount;
            //item.OnHoldCount += amount;

            //var eventMessage = new EventMessage
            //{
            //    Id = Guid.NewGuid(),
            //    MessageType = MessageType.Outbox,
            //    EventName = "OrderPlaced",
            //    Payload = JsonSerializer.Serialize(new PurchaseOrder() { Id = Guid.NewGuid(), ItemId = itemId }),
            //    CreatedAt = DateTime.UtcNow
            //};
            //_db.EventMessages.Add(eventMessage);
        }
    }
}
