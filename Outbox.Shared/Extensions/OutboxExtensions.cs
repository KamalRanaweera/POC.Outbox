
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Outbox.Shared.Controllers;
using Outbox.Shared.Interfaces;
using Outbox.Shared.Services;

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

        public static async Task UseSimpleMessageBroker(this IHost host, string simpleMessageBrokerRootUrl, string inboxEndpoint)
        {
            using (var scope = host.Services.CreateScope())
            {
                var messageBrokerAgent = scope.ServiceProvider.GetRequiredService<IMessageBrokerAgent>() as SimpleMessageBrokerAgent;
                if (messageBrokerAgent != null)
                    await messageBrokerAgent.RegisterInboxEndpoint("/inbox");
                else
                {
                    throw new Exception("Could not resolve IMessageBrokerAgent of type SimpleMessageBrokerAgent");
                }
            }

        }
    }
}
