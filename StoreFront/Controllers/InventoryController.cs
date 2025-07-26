using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Outbox.ProducerA.Models;
using Outbox.Shared;
using Outbox.Shared.Dtos;
using StoreFront.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace StoreFront.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InventoryController : ControllerBase
    {
        private readonly StoreFrontDbContext _context;

        public InventoryController(StoreFrontDbContext context)
        {
            _context = context;
        }

        // GET: api/Inventory
        [HttpGet]
        public async Task<ActionResult<IEnumerable<InventoryRecord>>> GetInventory()
        {
            return await _context.Inventory.ToListAsync();
        }

        [HttpPost("refill")]
        public async Task Refill()
        {
            var currentItems = await _context.Inventory.ToListAsync();
            foreach (var item in currentItems)
                item.AvailableCount = 10;

            for (int i=currentItems.Count;  i<10; i++)
                _context.Inventory.Add(new InventoryRecord()
                {
                    Name = $"Product {i}",
                    AvailableCount = 10
                });

            await _context.SaveChangesAsync();
        }

        [HttpPost("buy")]
        public async Task Buy(int itemId, int amount, bool priority = false)
        {
            using var tx = await _context.Database.BeginTransactionAsync();

            var item = await _context.Inventory.FirstOrDefaultAsync(x => x.Id == itemId);
            if (item == null)
                throw new Exception("Item not found");

            if (item.AvailableCount == 0)
                throw new Exception("Out of stock");

            if (item.AvailableCount < amount)
                throw new Exception("Insufficient stock");

            item.AvailableCount -= amount;
            item.OnHoldCount += amount;

            var outboxMessage = new OutboxMessage
            {
                Id = Guid.NewGuid(),
                EventType = "OrderPlaced",
                Payload = JsonSerializer.Serialize(new PurchaseOrder() { Id = Guid.NewGuid(), ItemId = itemId }),
                CreatedAt = DateTime.UtcNow
            };
            _context.OutboxMessages.Add(outboxMessage);

            await _context.SaveChangesAsync();

            await tx.CommitAsync();
        }


        // DELETE: api/Inventory/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteInventoryRecord(int id)
        {
            var inventoryRecord = await _context.Inventory.FindAsync(id);
            if (inventoryRecord == null)
            {
                return NotFound();
            }

            _context.Inventory.Remove(inventoryRecord);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool InventoryRecordExists(int id)
        {
            return _context.Inventory.Any(e => e.Id == id);
        }
    }
}
