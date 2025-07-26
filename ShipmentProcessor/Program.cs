using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Outbox.Shared.Controllers;
using Outbox.Shared.Extensions;
using Outbox.Shared.Interfaces;
using Outbox.Shared.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.ConfigureSimpleMessageBroker();

builder.Services.AddAuthorization();
builder.Services.AddControllers();

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
await app.UseSimpleMessageBroker("https://localhost:8000", "/inbox");

await app.RunAsync();
