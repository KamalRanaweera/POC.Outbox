using Microsoft.EntityFrameworkCore;
using Outbox.Shared;
using Outbox.Shared.Strategy.Abstractions;
using StoreFront.Models;

namespace StoreFront.Strategy.Implementations
{
    public class StoreFrontInboxMessageProcessor : IInboxMessageProcessor
    {
        private readonly StoreFrontDbContext _db;
        private readonly ILogger<StoreFrontInboxMessageProcessor> _logger;

        public StoreFrontInboxMessageProcessor(StoreFrontDbContext db, ILogger<StoreFrontInboxMessageProcessor> logger)
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

                Order order = _db.Orders.FirstOrDefault(x => x.Id == orderId)!;
                if (order == null)
                {
                    _logger.LogWarning($"ERROR: Order with ID {orderId} not found.");
                    return true; // Nothing else can be done, so this message can be considered processed.
                }


                if (message.EventName == EventNames.OrderInProgress)
                {
                    if (order.OrderStatus != OrderStatus.Completed)
                    {
                        order.OrderStatus = OrderStatus.InProgress;
                        Console.WriteLine($"{DateTime.Now}: Store Front Inbox: Marked order {message.Payload} in-progress");
                    }
                }
                else if (message.EventName == EventNames.OrderComplete)
                {
                    order.OrderStatus = OrderStatus.Completed;
                    Console.WriteLine($"{DateTime.Now}: Store Front Inbox: Marked order {message.Payload} completed");
                }

                await _db.SaveChangesAsync();
                return true;
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
    }
}
