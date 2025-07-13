using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Outbox.Shared;
using Outbox.Shared.Models;

namespace Outbox.OutboxShared.Services
{
    public interface IOutboxProcessorService
    {
        Task<List<OutboxMessage>> GetMessages();
        Task ProcessPendingMessagesAsync(CancellationToken cancellationToken);
        Task ProcessMessageByIdAsync(Guid messageId, CancellationToken cancellationToken);
    }


    public class OutboxProcessorService: IOutboxProcessorService
    {
        private readonly OutboxDbContext _dbContext;
        private readonly ILogger<OutboxProcessorService> _logger;

        public OutboxProcessorService(OutboxDbContext dbContext, ILogger<OutboxProcessorService> logger)
        {
            _dbContext = dbContext;
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
            {
                // Simulate processing
                _logger.LogInformation("Processing message {MessageId}", message.Id);

                message.Processed = true;
                message.ProcessedAt = DateTime.UtcNow;
            }

            await _dbContext.SaveChangesAsync(cancellationToken);
        }

        public async Task ProcessMessageByIdAsync(Guid messageId, CancellationToken cancellationToken)
        {
            var message = await _dbContext.OutboxMessages
                .FirstOrDefaultAsync(m => m.Id == messageId, cancellationToken);

            if (message is null) return;

            // Simulate processing
            _logger.LogInformation("Manually processing message {MessageId}", message.Id);

            message.Processed = true;
            message.ProcessedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync(cancellationToken);
        }
    }
}
