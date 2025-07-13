using Microsoft.EntityFrameworkCore;
using Outbox.Shared;

namespace Outbox.Shared.Models
{
    public class OutboxDbContext : DbContext
    {
        public OutboxDbContext(DbContextOptions options) : base(options) { }

        public DbSet<OutboxMessage> OutboxMessages => Set<OutboxMessage>();
    }
}
