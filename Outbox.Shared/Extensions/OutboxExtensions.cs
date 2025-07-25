using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Outbox.Shared.Interfaces;
using Outbox.Shared.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Outbox.Shared.Extensions
{
    public static class OutboxExtensions
    {
        //public static IHostApplicationBuilder ConfigureSimpleMessageBroker(this IHostApplicationBuilder builder)
        public static IHostApplicationBuilder ConfigureSimpleMessageBroker(this IHostApplicationBuilder builder)
        {

            //Registering the SimpleMessageBrokerAgent service
            builder.Services.AddScoped<IMessageBrokerAgent, SimpleMessageBrokerAgent>();

            return builder;
        }

        public static async Task UseSimpleMessageBroker(this IHost host, string simpleMessageBrokerRootUrl)
        {
            using (var scope = host.Services.CreateScope())
            {
                var messageBrokerAgent = scope.ServiceProvider.GetRequiredService<IMessageBrokerAgent>() as SimpleMessageBrokerAgent;
                if (messageBrokerAgent != null)
                    await messageBrokerAgent.RegisterInboxEndpoint();
                else
                {
                    throw new Exception("Could not resolve IMessageBrokerAgent of type SimpleMessageBrokerAgent");
                }
            }

        }
    }
}
