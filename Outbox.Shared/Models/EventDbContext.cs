using Microsoft.EntityFrameworkCore;
using Outbox.Shared;

namespace Outbox.Shared.Models
{
    public class EventDbContext : DbContext
    {
        public EventDbContext(DbContextOptions options) : base(options) { }

        public DbSet<EventMessage> EventMessages => Set<EventMessage>();
    }
}
