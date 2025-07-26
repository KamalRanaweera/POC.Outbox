using Microsoft.EntityFrameworkCore;
using StoreFront.Interfaces;
using StoreFront.Models;

namespace StoreFront.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly StoreFrontDbContext _db;
        public StoreFrontDbContext Db => _db;
        public InventoryService(StoreFrontDbContext db)
        {
            _db = db;
        }

        public async Task<List<InventoryRecord>> List()
        {
            return await _db.Inventory.ToListAsync();
        }

        public async Task AddItem(string name, int availableCount)
        {
            _db.Inventory.Add(new Models.InventoryRecord() { Name = name, AvailableCount = availableCount });
            await _db.SaveChangesAsync();
        }

        public async Task<bool> RemoveItem(int itemId)
        {
            var item = _db.Inventory.FirstOrDefault(x => x.Id == itemId);
            if (item == null)
                return false;

            _db.Remove(item);
            await _db.SaveChangesAsync();
            return true;
        }

        public async Task AddStock(int itemId, int quantity)
        {
            var item = _db.Inventory.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                item.AvailableCount += quantity;
                await _db.SaveChangesAsync();
            }
        }

        public async Task Purchase(int itemId, int quantity)
        {
            var item = _db.Inventory.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                if (item.AvailableCount < quantity)
                    throw new Exception($"Out of stock: {item.Name}");

                item.AvailableCount -= quantity;
                item.OnHoldCount += quantity;
                await _db.SaveChangesAsync();
            }
        }

        public async Task ResetQuantity(int count = 10, int maxItemTypes = 10)
        {
            foreach(var item in _db.Inventory)
                item.AvailableCount = count;

            for (int i = _db.Inventory.Count(); i < maxItemTypes; i++)
                _db.Inventory.Add(new InventoryRecord
                {
                    Name = $"Product {i + 1}",
                    AvailableCount = count,
                });

            await _db.SaveChangesAsync();
        }
    }
}
