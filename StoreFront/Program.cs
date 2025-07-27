using Hangfire;
using Microsoft.EntityFrameworkCore;
using Outbox.Shared.Extensions;
using Outbox.Shared.Interfaces;
using StoreFront.Interfaces;
using StoreFront.Models;
using StoreFront.Services;

var builder = WebApplication.CreateBuilder(args);

#region Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    builder.ConfigureSQLite<StoreFrontDbContext>("..\\data\\StoreFront.db");
else
    builder.ConfigureSqlServer<StoreFrontDbContext>(connectionString);
#endregion

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ConfigureSimpleMessageBrokerAgent();
builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddAutoMapper(typeof(Program));

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

// Configure recurrent Hangfire job to process event messages
RecurringJob.AddOrUpdate<IMessageProcessor>(
    "message-processor-job",
    processor => processor.ProcessMessagesAsync(),
    "*/10 * * * * *" // every 10 seconds
);

app.Run();
