using Hangfire;
using Microsoft.EntityFrameworkCore;
using Outbox.OutboxShared.Services;
using Outbox.Shared.Extensions;
using Outbox.Shared.Interfaces;
using Outbox.Shared.Models;
using Outbox.Shared.Strategy.Abstractions;
using StoreFront.Interfaces;
using StoreFront.Models;
using StoreFront.Services;
using StoreFront.Strategy.Implementations;

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
builder.Services.AddSwaggerGen(options => { options.UseInlineDefinitionsForEnums(); });

builder.ConfigureLogging();
builder.ConfigureSimpleMessageBrokerAgent();

builder.Services.AddScoped<IInventoryService, InventoryService>();
builder.Services.AddScoped<IOrderService, OrderService>();
builder.Services.AddScoped<IInboxMessageProcessor, StoreFrontInboxMessageProcessor>();

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
app.ScheduleHangfireRecurrentJobRunner("storefront-job-runner", "*/5 * * * * *"); // every 5 seconds


app.Run();
