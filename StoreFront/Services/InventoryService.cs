using Outbox.ProducerA.Models;

namespace StoreFront.Services
{
    public class InventoryService
    {
        private readonly StoreFrontDbContext _db;
        public InventoryService(StoreFrontDbContext db)
        {
            _db = db;
        }

        public void AddItem(string name, int availableCount)
        {
            _db.Inventory.Add(new Models.InventoryRecord() { Name = name, AvailableCount = availableCount });
            _db.SaveChanges();
        }

        public void RemoveItem(int itemId)
        {
            var item = _db.Inventory.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                _db.Remove(item);
                _db.SaveChanges();
            }
        }

        public void AddStock(int itemId, int quantity)
        {
            var item = _db.Inventory.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                item.AvailableCount += quantity;
                _db.SaveChanges();
            }
        }

        public void Purchase(int itemId, int quantity)
        {
            var item = _db.Inventory.FirstOrDefault(x => x.Id == itemId);
            if (item != null)
            {
                if (item.AvailableCount < quantity)
                    throw new Exception($"Out of stock: {item.Name}");

                item.AvailableCount -= quantity;
                item.OnHoldCount += quantity;
                _db.SaveChanges();
            }
        }

    }
}
