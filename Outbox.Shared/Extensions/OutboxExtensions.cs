
using Hangfire;
using Hangfire.Storage.SQLite;
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
    public static class OutboxExtensions
    {
        public static IHostApplicationBuilder ConfigureSimpleMessageBroker(this IHostApplicationBuilder builder)
        {
            //Registering the SimpleMessageBrokerAgent service
            builder.Services.AddScoped<IMessageBrokerAgent, SimpleMessageBrokerAgent>();

            builder.Services.AddHttpClient();

            return builder;
        }

        public static async Task UseSimpleMessageBroker(this IHost host, string inboxEndpoint)
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

        #region Database Configuration
        public static void ConfigureSqlServer<T>(this IHostApplicationBuilder builder, string connectionString) where T: DbContext
        {
            // EF Core (Outbox message persistence)
            builder.Services.AddDbContext<T>(options => options.UseSqlServer(connectionString));

            // Hangfire configuration
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSqlServerStorage(connectionString);
        }

        public static void ConfigureSQLite<T>(this IHostApplicationBuilder builder, string dbFile) where T: DbContext
        {
            var connectionString = $"\"Data Source={dbFile}";

            // EF Core (Outbox message persistence)
            builder.Services.AddDbContext<T>(options => options.UseSqlite(connectionString));

            // Hangfire configuration
            GlobalConfiguration.Configuration
                .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                .UseSimpleAssemblyNameTypeSerializer()
                .UseRecommendedSerializerSettings()
                .UseSQLiteStorage(connectionString);
        }
        #endregion
    }
}
