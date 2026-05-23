using OrderSystem.Application.DTOs;
using OrderSystem.Application.Interfaces;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Application.Services
{
    public class OrderService(IOrderRepository repository) : IOrderService
    {
        private readonly IOrderRepository _repository = repository;

        public async Task<Guid> CreateOrderAsync(CreateOrderDto dto)
        {
            var order = new Order
            {
                Id = Guid.NewGuid(),
                CustomerId = dto.CustomerId,
                Status = "Created",
                Total = dto.Total,
                CreatedAt = DateTime.UtcNow
            };

            return await _repository.CreateAsync(order);
        }

        public async Task<Order?> GetOrderAsync(Guid id) =>
            await _repository.GetByIdAsync(id);

        public async Task<List<Order>> GetAllOrdersAsync(int page = 1, int pageSize = 50) =>
            await _repository.GetAllAsync(page, pageSize);
    }
}
