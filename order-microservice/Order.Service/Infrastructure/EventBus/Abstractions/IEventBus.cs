using Order.Service.Infrastructure.EventBus;
namespace Order.Service.Infrastructure.EventBus.Abstractions;

public interface IEventBus
{
    Task PublishAsync(Event @event);
}