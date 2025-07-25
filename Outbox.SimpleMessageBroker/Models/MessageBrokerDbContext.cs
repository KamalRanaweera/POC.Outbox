using Microsoft.EntityFrameworkCore;

namespace Outbox.SimpleMessageBroker.Models
{
    public class MessageBrokerDbContext: DbContext
    {
        public MessageBrokerDbContext(DbContextOptions<MessageBrokerDbContext> options) : base(options) { }
        public DbSet<MessageConsumer> Consumers => Set<MessageConsumer>();
    }
}
