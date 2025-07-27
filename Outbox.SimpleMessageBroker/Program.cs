using Hangfire;
using Microsoft.EntityFrameworkCore;
using Outbox.Shared.Extensions;
using Outbox.Shared.Strategy.Abstractions;
using Outbox.SimpleMessageBroker.Models;
using Outbox.SimpleMessageBroker.Strategy.Implementations;

var builder = WebApplication.CreateBuilder(args);

#region Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString))
    builder.ConfigureSQLite<MessageBrokerDbContext>("..\\data\\SimpleMessageBroker.db");
else
    builder.ConfigureSqlServer<MessageBrokerDbContext>(connectionString);
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ConfigureSimpleMessageBrokerAgent();

builder.Services.AddScoped<IInboxMessageProcessor, SimpleMessageBrokerInboxMessageProcessor>();

builder.Services.AddHangfireServer();

var app = builder.Build();
app.UseHangfireDashboard("/hangfire");
app.ScheduleHangfireRecurrentJobRunner("simple-message-broker-job-runner", "*/10 * * * * *"); // every 10 seconds

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
