using Microsoft.EntityFrameworkCore;
using Outbox.Shared.Extensions;
using StoreFront.Models;

var builder = WebApplication.CreateBuilder(args);

#region Database Configuration
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString)) // Use Sqlite
    builder.ConfigureSQLite<StoreFrontDbContext>("..\\data\\StoreFront.db");
else // Use SQL Server
    builder.ConfigureSqlServer<StoreFrontDbContext>(connectionString);
#endregion

builder.Services.AddAuthorization();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ConfigureSimpleMessageBroker();

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

// Register at the message broker for receiving messsages
await app.UseSimpleMessageBroker("/inbox");

app.Run();
