using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Outbox.Shared.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outbox.Shared.Services
{
    public class SimpleMessageBrokerAgent : IMessageBrokerAgent
    {
        private readonly IConfiguration _configuration;
        private readonly string _inboxEndpoint;
        public SimpleMessageBrokerAgent(IConfiguration configuration)
        {
            _configuration = configuration;
            var siteRoot = _configuration.GetSection("Outbox:ReceiverInboxEndpoint").Value!.TrimEnd('/');
            _inboxEndpoint = $"{siteRoot}/inbox";
        }

        public async Task Publish(string eventName, object payload)
        {

        }

        public async Task RegisterInboxEndpoint()
        {
            Console.WriteLine($"Registered: {_inboxEndpoint}");
        }
    }
}
