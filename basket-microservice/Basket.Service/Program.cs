using Basket.Service.Endpoints;
using Basket.Service.Infrastructure.Data;
using Basket.Service.Infrastructure.RabbitMq;
using ECommerce.Shared.Infrastructure.EventBus;
using ECommerce.Shared.Infrastructure.RabbitMq;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IBasketStore, InMemoryBasketStore>();
builder.Services.AddRabbitMqEventBus(builder.Configuration)
    .AddRabbitMqSubscriberService(builder.Configuration)
    .AddEventHandler<OrderCreatedEvent, OrderCreatedEventHandler>();

builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

app.RegisterEndpoints();

app.UseHttpsRedirection();

app.Run();