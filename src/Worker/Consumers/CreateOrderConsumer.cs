using MassTransit;
using OrderSystem.Application.DTOs;
using OrderSystem.Application.Interfaces;

namespace OrderSystem.Worker.Consumers;

public class CreateOrderConsumer(IOrderService orderService, ILogger<CreateOrderConsumer> logger) : IConsumer<CreateOrderDto>
{
    private readonly IOrderService _orderService = orderService;
    private readonly ILogger<CreateOrderConsumer> _logger = logger;

    public async Task Consume(ConsumeContext<CreateOrderDto> context)
    {
        _logger.LogInformation(
            "Procesando orden para cliente {CustomerId} (intento #{RetryCount})",
            context.Message.CustomerId,
            context.GetRetryAttempt());

        try
        {
            var orderId = await _orderService.CreateOrderAsync(context.Message);

            _logger.LogInformation(
                "Orden {OrderId} procesada exitosamente", orderId);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Error al procesar orden para cliente {CustomerId}. El mensaje será reintentado.",
                context.Message.CustomerId);

            // Re-lanzar la excepción para que MassTransit aplique la política de reintentos.
            // Si se agotan los reintentos, el mensaje va automáticamente a "orders-queue_error".
            throw;
        }
    }
}