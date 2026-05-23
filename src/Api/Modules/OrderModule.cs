using Carter;
using MassTransit;
using OrderSystem.Application.DTOs;
using OrderSystem.Application.Interfaces;

namespace OrderSystem.Api.Modules
{
    public class OrderModule : ICarterModule
    {
        public void AddRoutes(IEndpointRouteBuilder app)
        {
            app.MapPost("/api/orders", async (CreateOrderDto dto, IPublishEndpoint publisher) =>
            {
                await publisher.Publish(dto);
                return Results.Accepted();
            });

            app.MapGet("/api/orders/{id:guid}", async (Guid id, IOrderService service) =>
            {
                var order = await service.GetOrderAsync(id);
                return order is null ? Results.NotFound() : Results.Ok(order);
            });

            app.MapGet("/api/orders", async (IOrderService service, int page = 1, int pageSize = 50) =>
            {
                if (pageSize > 100) pageSize = 100;
                var orders = await service.GetAllOrdersAsync(page, pageSize);
                return Results.Ok(orders);
            });
        }
    }
}
