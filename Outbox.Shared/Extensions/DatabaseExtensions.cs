
using Hangfire;
using Hangfire.Storage.SQLite;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Outbox.Shared.Models;

namespace Outbox.Shared.Extensions
{
    public static class DatabaseExtensions
    {
        public static void ConfigureSqlServer<T>(this IHostApplicationBuilder builder, string connectionString, bool useHangfire = true) where T: EventDbContext
        {
            // EF Core (Outbox message persistence)
            builder.Services.AddDbContext<T>(options => options.UseSqlServer(connectionString));

            // Register the base type explicitly using the derived instance.
            // Otherwise, it fails to inject MessageProcessor which depends on the EventDbContext base type.
            builder.Services.AddScoped<EventDbContext>(provider =>
                provider.GetRequiredService<T>());

            if (useHangfire)
                builder.Services.AddHangfire(configuration =>
                {
                    configuration
                    .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                    .UseSimpleAssemblyNameTypeSerializer()
                    .UseRecommendedSerializerSettings()
                    .UseSqlServerStorage(connectionString);
                });
        }

        public static void ConfigureSQLite<T>(this IHostApplicationBuilder builder, string dbFile, bool useHangfire = true) where T: EventDbContext
        {
            var connectionString = $"\"Data Source={dbFile}";

            // EF Core (Outbox message persistence)
            builder.Services.AddDbContext<T>(options => options.UseSqlite(connectionString));

            // Register the base type explicitly using the derived instance.
            // Otherwise, it fails to inject MessageProcessor which depends on the EventDbContext base type.
            builder.Services.AddScoped<EventDbContext>(provider =>
                provider.GetRequiredService<T>());

            if (useHangfire)
                builder.Services.AddHangfire(configuration =>
                {
                    configuration
                        .SetDataCompatibilityLevel(CompatibilityLevel.Version_180)
                        .UseSimpleAssemblyNameTypeSerializer()
                        .UseRecommendedSerializerSettings()
                        .UseSQLiteStorage(connectionString);
                });
        }
    }
}
