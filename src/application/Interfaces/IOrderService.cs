using OrderSystem.Application.DTOs;
using OrderSystem.Domain.Entities;

namespace OrderSystem.Application.Interfaces
{
    public interface IOrderService
    {
        Task<Guid> CreateOrderAsync(CreateOrderDto dto);
        Task<Order?> GetOrderAsync(Guid id);
        Task<List<Order>> GetAllOrdersAsync(int page = 1, int pageSize = 50);
    }
}
