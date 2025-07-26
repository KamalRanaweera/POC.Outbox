using Hangfire;
using Microsoft.EntityFrameworkCore;
using Outbox.Shared.Extensions;
using Outbox.SimpleMessageBroker.Models;

var builder = WebApplication.CreateBuilder(args);

#region Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString)) // Use Sqlite
    builder.ConfigureSQLite<MessageBrokerDbContext>("..\\data\\SimpleMessageBroker.db");
else // Use SQL Server
    builder.ConfigureSqlServer<MessageBrokerDbContext>(connectionString);
#endregion

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

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
