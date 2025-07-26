using Microsoft.EntityFrameworkCore;
using Outbox.Shared.Models;

namespace Outbox.SimpleMessageBroker.Models
{
    public class MessageBrokerDbContext: EventDbContext
    {
        public MessageBrokerDbContext(DbContextOptions<MessageBrokerDbContext> options) : base(options) { }

        public DbSet<MessageConsumer> Consumers => Set<MessageConsumer>();
    }
}
