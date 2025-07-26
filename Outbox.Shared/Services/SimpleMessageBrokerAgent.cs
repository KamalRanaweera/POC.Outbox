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
        private readonly string _siteRoot;
        public SimpleMessageBrokerAgent(IConfiguration configuration)
        {
            _configuration = configuration;
            _siteRoot = _configuration.GetSection("Outbox:AppSiteRoot").Value!.TrimEnd('/');
        }

        public async Task Publish(string eventName, object payload)
        {

        }

        public async Task RegisterInboxEndpoint(string inboxEndpoint)
        {
            var inboxEndpointUrl = $"{_siteRoot}/{inboxEndpoint.Trim('/')}";
            Console.WriteLine($"Registered: {inboxEndpointUrl}");
        }
    }
}
