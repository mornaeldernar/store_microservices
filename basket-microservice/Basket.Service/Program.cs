using Basket.Service.Endpoints;
using Basket.Service.Infrastructure.Data;
using Basket.Service.Infrastructure.RabbitMq;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddScoped<IBasketStore, InMemoryBasketStore>();
builder.Services.AddRabbitMqEventBus(builder.Configuration);
builder.Services.AddHostedService<RabbitMqHostedService>();

var app = builder.Build();

app.RegisterEndpoints();

app.UseHttpsRedirection();

app.Run();