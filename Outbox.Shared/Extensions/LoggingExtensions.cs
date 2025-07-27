using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;

namespace Outbox.Shared.Extensions
{
    public static class LoggingExtensions
    {
        public static IHostApplicationBuilder ConfigureLogging(this IHostApplicationBuilder builder)
        {
            #region Serilog Configuration
            Log.Logger = new LoggerConfiguration()
                .ReadFrom.Configuration(builder.Configuration) // reads from appsettings.json
                .Enrich.FromLogContext()
                .WriteTo.Console()
                .WriteTo.File("Logs/log-.txt", rollingInterval: RollingInterval.Day)
                .CreateLogger();

            // Attach Serilog to logging pipeline
            builder.Logging.ClearProviders();
            builder.Logging.AddSerilog();
            #endregion

            #region Configure Console Standar Output to a File
            var consoleLogDir = "Logs";
            Directory.CreateDirectory(consoleLogDir);
            var logFilePath = Path.Combine(consoleLogDir, "console.log");
            var fileStream = new FileStream(logFilePath, FileMode.Append, FileAccess.Write, FileShare.ReadWrite);
            var writer = new StreamWriter(fileStream)
            {
                AutoFlush = true
            };
            Console.SetOut(writer); // Redirects Console.WriteLine
            #endregion


            return builder;
        }
  }
}
