using Microsoft.EntityFrameworkCore;
using Outbox.ProducerA.Models;
using Outbox.Shared.Extensions;
using Outbox.Shared.Interfaces;
using Outbox.Shared.Services;
using StoreFront.Controllers;
using StoreFront.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString)) // Use Sqlite
{
    var dbFile = "..\\data\\StoreFront.db";
    connectionString = $"\"Data Source={dbFile}";

    // EF Core (Outbox message persistence)
    builder.Services.AddDbContext<StoreFrontDbContext>(options =>
        options.UseSqlite(connectionString));
}
else // Use SQL Server
{
    // EF Core (Outbox message persistence)
    builder.Services.AddDbContext<StoreFrontDbContext>(options =>
        options.UseSqlServer(connectionString));
}

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
