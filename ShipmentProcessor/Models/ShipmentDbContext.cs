using Microsoft.EntityFrameworkCore;
using Outbox.Shared;
using Outbox.Shared.Models;

namespace ShipmentProcessor.Models
{
    public class ShipmentDbContext : OutboxDbContext
    {
        public ShipmentDbContext(DbContextOptions<ShipmentDbContext> options) : base(options) { }

        public DbSet<Shipment> Shipments => Set<Shipment>();

    }
}
