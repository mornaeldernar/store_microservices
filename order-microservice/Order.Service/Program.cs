using Order.Service.Endpoints;
using Order.Service.Infrastructure.Data;
using Order.Service.Infrastructure.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRabbitMqEventBus(builder.Configuration);

builder.Services.AddScoped<IOrderStore, InMemoryOrderStore>();

var app = builder.Build();

app.RegisterEndpoints();
app.UseHttpsRedirection();

app.Run();
