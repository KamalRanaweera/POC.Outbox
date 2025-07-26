using StoreFront.Models;

namespace StoreFront.Interfaces
{
    public interface IInventoryService
    {
        public Task<List<InventoryRecord>> List();
        public Task AddItem(string name, int availableCount);
        public Task<bool> RemoveItem(int itemId);
        public Task AddStock(int itemId, int quantity);
        public Task Purchase(int itemId, int quantity);
        public Task ResetQuantity(int count = 10, int maxItemTypes = 10);
    }
}
