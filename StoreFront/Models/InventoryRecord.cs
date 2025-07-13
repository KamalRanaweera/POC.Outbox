using Azure.Core.Pipeline;

namespace StoreFront.Models
{
    public class InventoryRecord
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public int AvailableCount {  get; set; }
        public int OnHoldCount {  get; set; }
        public int SoldCount {  get; set; }
    }
}
