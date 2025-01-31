using Microsoft.AspNetCore.Mvc;
using Order.Service.Infrastructure.Data;
using Order.Service.ApiModels;
using ECommerce.Shared.Infrastructure.EventBus.Abstractions;
using Order.Service.IntegrationEvents.Events;
using ECommerce.Shared.Observability.Metrics;

namespace Order.Service.Endpoints;
public static class OrderApiEndpoints
{
    public static void RegisterEndpoints(this IEndpointRouteBuilder routeBuilder)
    {
        routeBuilder.MapPost("/{customerId}", async
            ([FromServices] IEventBus eventBus,
                [FromServices] IOrderStore orderStore, [FromServices] MetricFactory metricFactory,
            string customerId, CreateOrderRequest request) =>
            {
                var order = new Models.Order
                {
                    CustomerId = customerId
                };
                foreach (var product in request.OrderProducts)
                {
                    order.AddOrderProduct(product.ProductId, product.Quantity);
                }
                await orderStore.CreateOrder(order);

                var orderCounter = metricFactory.Counter("total-orders", "Orders");
                orderCounter.Add(1);

                var productsPerOrderHistogram = metricFactory.Histogram("products-per-order", "Products");
                productsPerOrderHistogram.Record(order.OrderProducts.DistinctBy(p => p.ProductId).Count());

                await eventBus.PublishAsync(new OrderCreatedEvent(customerId));
                return TypedResults.Created($"{order.CustomerId}/{order.OrderId}");
            });

        routeBuilder.MapGet("/{customerId}/{orderId}", async Task<IResult>
            ([FromServices] IOrderStore orderStore,
                string customerId,
                string orderId
            ) =>
            {
                var order = await orderStore.GetCustomerOrderById(customerId, orderId);
                return order is null
                    ? TypedResults.NotFound("Order not found for customer")
                    : TypedResults.Ok(new GetOrderResponse(order.CustomerId, order.OrderId, order.OrderDate,
                    order.OrderProducts.Select(op =>
                    new GetOrderProductResponse(op.ProductId, op.Quantity)).ToList()));
            }
        );
    }


}