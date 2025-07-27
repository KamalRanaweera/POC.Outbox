using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
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
            _messageBrokerSubscriberListEndpoint;

        private readonly string _messageConsumerRoot;

        private readonly HttpClient _httpClient;

        public SimpleMessageBrokerAgent(IConfiguration configuration, IHttpClientFactory httpClientFactory)
        {
            _configuration = configuration;

            var messageBrokerRoot = _configuration.GetSection("Outbox:MessageBrokerRoot").Value?.TrimEnd('/')!;
            _messageBrokerSubscribeEndpoint = $"{messageBrokerRoot}/api/consumers/subscribe";
            _messageBrokerUnsubscribeEndpoint = $"{messageBrokerRoot}/api/consumers/unsubscribe";
            _messageBrokerSubscriberListEndpoint = $"{messageBrokerRoot}/api/consumers/list";

            _messageConsumerRoot = _configuration.GetSection("Outbox:MessageConsumerRoot").Value?.TrimEnd('/')!;

            _httpClient = httpClientFactory.CreateClient();
        }

        public async Task<bool> Publish(EventMessage message)
        {
            return false;
        }

        public async Task SubscribeToMessages(string inboxEndpoint)
        {
            var inboxEndpointUrl = $"{_messageConsumerRoot}/api/{inboxEndpoint.Trim('/')}";
            var content = new StringContent(JsonSerializer.Serialize(inboxEndpointUrl), Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync(_messageBrokerSubscribeEndpoint, content);
            Console.WriteLine($"Subscribe: {inboxEndpointUrl} at {_messageBrokerSubscribeEndpoint} => {response.StatusCode}");
        }
    }
}
