namespace StoreFront.Models
{
    public class OrderItem
    {
        public int Id { get; set; }
        public int OrderId {  get; set; }
        public Order Order { get; set; } = null!;
        public int InventoryRecordId {  get; set; }
        public InventoryRecord InventoryRecord { get; set; } = null!;
        public int Count {  get; set; }
    }
}
