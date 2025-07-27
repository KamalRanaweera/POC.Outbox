namespace StoreFront.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; } = string.Empty;
        public string CustomerAddress { get; set; } = string.Empty;
        public List<OrderItem> Items { get; set; } = [];
        public OrderStatus OrderStatus {  get; set; }
    }

    public enum OrderStatus { Open = 0, InProgress, Completed }


}
