using Hangfire;
using Outbox.Shared.Extensions;
using Outbox.Shared.Strategy.Abstractions;
using ShipmentProcessor.Interfaces;
using ShipmentProcessor.Models;
using ShipmentProcessor.Services;
using ShipmentProcessor.Strategy.Implementations;

var builder = WebApplication.CreateBuilder(args);

#region Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    builder.ConfigureSQLite<ShipmentDbContext>("..\\data\\ShipmentProcessor.db");
else
    builder.ConfigureSqlServer<ShipmentDbContext>(connectionString);
#endregion


// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ConfigureSimpleMessageBrokerAgent();

builder.Services.AddScoped<IInboxMessageProcessor, ShipmentProcessorInboxMessageProcessor>();
builder.Services.AddScoped<IShipmentService, ShipmentService>();

builder.Services.AddAutoMapper(typeof(Program));

builder.Services.AddAuthorization();
builder.Services.AddControllers();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
    app.UseHangfireDashboard("/hangfire");
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

// Register at the message broker for receiving messsages
await app.UseSimpleMessageBrokerAgent("/inbox");
app.ScheduleHangfireRecurrentJobRunner("shipment-processor-job-runner", "*/10 * * * * *"); // every 10 seconds

await app.RunAsync();
