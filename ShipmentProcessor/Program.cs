using Hangfire;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Outbox.Shared.Controllers;
using Outbox.Shared.Extensions;
using Outbox.Shared.Interfaces;
using Outbox.Shared.Services;
using ShipmentProcessor.Models;

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

await app.RunAsync();
