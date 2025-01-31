using ECommerce.Shared.Infrastructure.EventBus;

namespace Order.Service.Infrastructure.Data.EntityFramework
{
    public record OrderCreatedEvent(string CustomerId) : Event;
}
