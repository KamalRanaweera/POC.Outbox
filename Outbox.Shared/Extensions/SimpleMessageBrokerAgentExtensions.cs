
using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

using Outbox.Shared.Controllers;
using Outbox.Shared.Interfaces;
using Outbox.Shared.Services;

namespace Outbox.Shared.Extensions
{
    public static class SimpleMessageBrokerAgentExtensions
    {
        public static IHostApplicationBuilder ConfigureSimpleMessageBrokerAgent(this IHostApplicationBuilder builder)
        {
            //Registering the SimpleMessageBrokerAgent service
            builder.Services.AddScoped<IMessageBrokerAgent, SimpleMessageBrokerAgent>();

            builder.Services.AddHttpClient();

            return builder;
        }

        public static async Task UseSimpleMessageBrokerAgent(this IHost host, string inboxEndpoint)
        {
            using (var scope = host.Services.CreateScope())
            {
                var messageBrokerAgent = scope.ServiceProvider.GetRequiredService<IMessageBrokerAgent>() as SimpleMessageBrokerAgent;
                if (messageBrokerAgent != null)
                    await messageBrokerAgent.SubscribeToMessages(inboxEndpoint ?? "/inbox");
                else
                {
                    throw new Exception("Could not resolve IMessageBrokerAgent of type SimpleMessageBrokerAgent");
                }
            }

        }
    }
}
