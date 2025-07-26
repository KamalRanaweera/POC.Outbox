using Microsoft.EntityFrameworkCore;
using Outbox.Shared;
using Outbox.Shared.Models;

namespace StoreFront.Models
{
    public class StoreFrontDbContext : EventDbContext
    {
        public StoreFrontDbContext(DbContextOptions<StoreFrontDbContext> options) : base(options) { }

        public DbSet<InventoryRecord> Inventory => Set<InventoryRecord>();
        public DbSet<OrderItem> OrderItems => Set<OrderItem>();
        public DbSet<Order> Orders => Set<Order>();
    }
}
