using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Outbox.Shared.Extensions
{
    public static class LoggingExtensions
    {
        public static IHostApplicationBuilder ConfigureLogging(this IHostApplicationBuilder builder)
        {
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration) // reads from appsettings.json
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Attach Serilog to logging pipeline
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();

            return builder;
        }
  }
}
