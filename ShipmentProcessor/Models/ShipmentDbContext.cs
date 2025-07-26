using Microsoft.EntityFrameworkCore;
using Outbox.Shared;
using Outbox.Shared.Models;

namespace ShipmentProcessor.Models
{
    public class ShipmentDbContext : EventDbContext
    {
        public ShipmentDbContext(DbContextOptions<ShipmentDbContext> options) : base(options) { }

    }
}
