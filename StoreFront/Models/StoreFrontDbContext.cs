using Microsoft.EntityFrameworkCore;
using Outbox.Shared;
using Outbox.Shared.Models;
using StoreFront.Models;

namespace Outbox.ProducerA.Models
{
    public class StoreFrontDbContext : OutboxDbContext
    {
        public StoreFrontDbContext(DbContextOptions<StoreFrontDbContext> options) : base(options) { }
        public DbSet<InventoryRecord> Inventory => Set<InventoryRecord>();

    }
}
