using Hangfire.States;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Outbox.Shared;
using Outbox.Shared.Interfaces;
using Outbox.Shared.Models;
using Outbox.Shared.Strategy.Abstractions;

namespace Outbox.OutboxShared.Services
{
    public class MessageProcessor: IMessageProcessor
    {
        private readonly EventDbContext _dbContext;
        private readonly IMessageBrokerAgent _messageBrokerAgent;
        private readonly IInboxMessageProcessor _inboxMessageProcessor;
        private readonly ILogger<MessageProcessor> _logger;

        public MessageProcessor(EventDbContext dbContext, IMessageBrokerAgent messageBrokerAgent, IInboxMessageProcessor inboxMessageProcessor, ILogger<MessageProcessor> logger)
        {
            _dbContext = dbContext;
            _messageBrokerAgent = messageBrokerAgent;
            _inboxMessageProcessor = inboxMessageProcessor;
            _logger = logger;
        }

        public async Task ProcessMessagesAsync()
        {
            var messages = await _dbContext.EventMessages
                .Where(m => !m.Processed)
                .ToListAsync();

            foreach (var message in messages)
                await ProcessMessageAsync(message);

            await _dbContext.SaveChangesAsync();
        }

        public async Task ProcessMessageByIdAsync(Guid messageId)
        {
            var message = await _dbContext.EventMessages
                .FirstOrDefaultAsync(m => m.Id == messageId);

            if (message is null)
                return;

            await ProcessMessageAsync(message);
            await _dbContext.SaveChangesAsync();
        }

        private async Task ProcessMessageAsync(EventMessage message)
        {
            if(message.MessageType == MessageType.Outbox)
                message.Processed = await _messageBrokerAgent.Publish(message);
            else
                message.Processed = await _inboxMessageProcessor.ProcessMessageAsync(message);

            message.ProcessedAt = DateTime.UtcNow;

        }
    }
}
