using Microsoft.EntityFrameworkCore;
using Outbox.SimpleMessageBroker.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
if (string.IsNullOrEmpty(connectionString)) // Use Sqlite
{
    var dbFile = "..\\data\\SimpleMessageBroker.db";
    connectionString = $"\"Data Source={dbFile}";

    // EF Core (Outbox message persistence)
    builder.Services.AddDbContext<MessageBrokerDbContext>(options =>
        options.UseSqlite(connectionString));
}
else // Use SQL Server
{
    // EF Core (Outbox message persistence)
    builder.Services.AddDbContext<MessageBrokerDbContext>(options =>
        options.UseSqlServer(connectionString));
    builder.Services.AddControllers();
}

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
