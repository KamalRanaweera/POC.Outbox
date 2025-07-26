
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
    public static class DatabaseExtensions
    {
        public static void ConfigureSqlServer<T>(this IHostApplicationBuilder builder, string connectionString, bool useHangfire = true) where T: DbContext
        {
            // EF Core (Outbox message persistence)
            builder.Services.AddDbContext<T>(options => options.UseSqlServer(connectionString));

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

        public static void ConfigureSQLite<T>(this IHostApplicationBuilder builder, string dbFile, bool useHangfire = true) where T: DbContext
        {
            var connectionString = $"\"Data Source={dbFile}";

            // EF Core (Outbox message persistence)
            builder.Services.AddDbContext<T>(options => options.UseSqlite(connectionString));

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
