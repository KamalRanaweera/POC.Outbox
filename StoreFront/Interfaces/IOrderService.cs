using StoreFront.Models;

namespace StoreFront.Interfaces
{
    public interface IOrderService
    {
        public Task<bool> PlaceOrder(Order order);
    }
}
