using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Outbox.Shared;
using Outbox.Shared.Dtos;
using Outbox.Shared.Strategy.Abstractions;
using Outbox.SimpleMessageBroker.Models;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace Outbox.SimpleMessageBroker.Strategy.Implementations
{
    public class SimpleMessageBrokerInboxMessageProcessor : IInboxMessageProcessor
    {
        private readonly MessageBrokerDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly HttpClient _httpClient;
        private readonly ILogger<SimpleMessageBrokerInboxMessageProcessor> _logger;
        public SimpleMessageBrokerInboxMessageProcessor(MessageBrokerDbContext dbContext, IMapper mapper, IHttpClientFactory httpClientFactory, ILogger<SimpleMessageBrokerInboxMessageProcessor> logger)
        {
            _dbContext = dbContext;
            _mapper = mapper;
            _httpClient = httpClientFactory.CreateClient();
            _logger = logger;
        }

        public async Task<bool> ProcessMessageAsync(EventMessage message)
        {
            Console.WriteLine($"{DateTime.Now}: Message Broker Inbox: <{message.EventName}, {message.Payload}>");

            var consumers = await _dbContext.Consumers
                //.Where(c => c.Id != message.SenderId)
                .ToListAsync();

            var messageDto = _mapper.Map<EventMessageDto>(message);
            bool success = true;
            foreach (var consumer in consumers)
                success &= await PostToConsumer(consumer, messageDto);


            await Task.CompletedTask;
            return success;
        }

        private async Task<bool> PostToConsumer(MessageConsumer consumer, EventMessageDto messageDto)
        {
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(messageDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(consumer.Endpoint, content);
                Console.WriteLine($"{DateTime.Now}: PostToConsumer: {consumer.Endpoint} {JsonSerializer.Serialize(messageDto)} => {response.StatusCode}");
                return response.StatusCode == System.Net.HttpStatusCode.OK;
            }
            catch (Exception)
            {
                // Failed to deliver the message 
                return false;
            }
        }
    }
}
