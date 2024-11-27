using RabbitMQ.Client;
using RabbitMQ.Client.Events;
using System.Text;
using System.Text.Json;
using Basket.Service.IntegrationEvents;
using Basket.Service.Infrastructure.Data;

namespace Basket.Service.Infrastructure.RabbitMq;

public class RabbitMqHostedService : IHostedService
{
    private readonly IServiceProvider _serviceProvider;
    public RabbitMqHostedService(IServiceProvider serviceProvider){
        _serviceProvider = serviceProvider;
    }
    public Task StartAsync(CancellationToken cancellationToken)
    {
        _ = Task.Factory.StartNew(() => 
        {
            var rabbitMQConnection = _serviceProvider.GetRequiredService<IRabbitMqConnection>();
            var channel = rabbitMQConnection.Connection.CreateModel();

            channel.QueueDeclare(
                queue: nameof(OrderCreatedEvent),
                durable: false,
                exclusive:false,
                autoDelete: false,
                arguments: null);

            var consumer = new EventingBasicConsumer(channel);
            consumer.Received += OnMessageReceived;

            channel.BasicConsume(
                queue: nameof(OrderCreatedEvent),
                autoAck: true,
                consumer: consumer
            );
        },
        TaskCreationOptions.LongRunning);
        return Task.CompletedTask;

    }
    private void OnMessageReceived(object? sender, BasicDeliverEventArgs eventArgs)
    {
        var message = Encoding.UTF8.GetString(eventArgs.Body.Span);
        var @event = JsonSerializer.Deserialize(message, typeof(OrderCreatedEvent)) as OrderCreatedEvent;

        using var scope = _serviceProvider.CreateScope();
        
        var basketStore = scope.ServiceProvider.GetRequiredService<IBasketStore>();

        basketStore.DeleteCustomerBasket(@event.CustomerId);
    }
    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}