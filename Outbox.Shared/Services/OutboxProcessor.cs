using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Outbox.Shared;
using Outbox.Shared.Dtos;
using Outbox.Shared.Interfaces;
using Outbox.Shared.Models;

namespace Outbox.OutboxShared.Services
{
    public class OutboxProcessor: IOutboxProcessor
    {
        private readonly OutboxDbContext _dbContext;
        private readonly IMessageBrokerAgent _messageBrokerAgent;
        private readonly ILogger<OutboxProcessor> _logger;

        public OutboxProcessor(OutboxDbContext dbContext, IMessageBrokerAgent messageBrokerAgent, ILogger<OutboxProcessor> logger)
        {
            _dbContext = dbContext;
            _messageBrokerAgent = messageBrokerAgent;
            _logger = logger;
        }

        public async Task<List<OutboxMessage>> GetMessages()
        {
            return await _dbContext.OutboxMessages
                .Where(m => !m.Processed)
                .ToListAsync();
        }

        public async Task ProcessPendingMessagesAsync(CancellationToken cancellationToken)
        {
            var messages = await _dbContext.OutboxMessages
                .Where(m => !m.Processed)
                .ToListAsync(cancellationToken);

            foreach (var message in messages)
                await ProcessMessageAsync(message);

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task ProcessMessageByIdAsync(Guid messageId, CancellationToken cancellationToken)
        {
            var message = await _dbContext.OutboxMessages
                .FirstOrDefaultAsync(m => m.Id == messageId, cancellationToken);

            if (message is null)
                return;

            await ProcessMessageAsync(message);
            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        private async Task ProcessMessageAsync(OutboxMessage message)
        {
            bool processingStatus = await _messageBrokerAgent.Publish(message.EventType, message.Payload);
            message.Processed = processingStatus;
            message.ProcessedAt = DateTime.UtcNow;
        }
    }
}
