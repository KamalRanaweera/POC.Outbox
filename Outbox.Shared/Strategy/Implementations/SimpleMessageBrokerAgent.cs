using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Outbox.Shared.Dtos;
using Outbox.Shared.Strategy.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Outbox.Shared.Strategy.Implementations
{
    public class SimpleMessageBrokerAgent : IMessageBrokerAgent
    {
        private readonly IConfiguration _configuration;

        private readonly string
            _messageBrokerSubscribeEndpoint,
            _messageBrokerUnsubscribeEndpoint,
            _messageBrokerSubscriberListEndpoint,
            _messageBrokerPublishEndpoint;

        private readonly string _messageConsumerRoot;

        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly ILogger<SimpleMessageBrokerAgent> _logger;

        public SimpleMessageBrokerAgent(IConfiguration configuration, IHttpClientFactory httpClientFactory, IMapper mapper, ILogger<SimpleMessageBrokerAgent> logger)
        {
            _configuration = configuration;

            #region Simple MessageBbroker endpoints
            var messageBrokerRoot = _configuration.GetSection("Outbox:MessageBrokerRoot").Value?.TrimEnd('/')!;
            _messageBrokerSubscribeEndpoint = $"{messageBrokerRoot}/api/consumers/subscribe";
            _messageBrokerUnsubscribeEndpoint = $"{messageBrokerRoot}/api/consumers/unsubscribe";
            _messageBrokerSubscriberListEndpoint = $"{messageBrokerRoot}/api/consumers/list";
            _messageBrokerPublishEndpoint = $"{messageBrokerRoot}/api/publish";
            #endregion

            _messageConsumerRoot = _configuration.GetSection("Outbox:MessageConsumerRoot").Value?.TrimEnd('/')!;

            _httpClient = httpClientFactory.CreateClient();

            _mapper = mapper;
            _logger = logger;
        }

        public async Task<bool> PublishToBroker(EventMessage message)
        {
            var messageDto = _mapper.Map<EventMessageDto>(message);
            try
            {
                var content = new StringContent(JsonSerializer.Serialize(messageDto), Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(_messageBrokerPublishEndpoint, content);
                Console.WriteLine($"{DateTime.Now}: PublishToBroker: {JsonSerializer.Serialize(messageDto)} => {response.StatusCode}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
                return false;
            }
        }

        public async Task SubscribeToMessages(string inboxEndpoint)
        {
            try
            {
                var inboxEndpointUrl = $"{_messageConsumerRoot}/api/{inboxEndpoint.Trim('/')}";
                var content = new StringContent(JsonSerializer.Serialize(inboxEndpointUrl), Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(_messageBrokerSubscribeEndpoint, content);
                Console.WriteLine($"{DateTime.Now}: Subscribe: {inboxEndpointUrl} at {_messageBrokerSubscribeEndpoint} => {response.StatusCode}");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex.Message);
            }
        }
    }
}
