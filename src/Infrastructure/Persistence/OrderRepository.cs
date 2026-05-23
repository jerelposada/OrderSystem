using Microsoft.EntityFrameworkCore;
using OrderSystem.Application.Interfaces;
using OrderSystem.Domain.Entities;


namespace OrderSystem.Infrastructure.Persistence
{
    public class OrderRepository(AppDbContext context): IOrderRepository
    {
        public async Task<Guid> CreateAsync(Order order)
        {
            context.Orders.Add(order);
            await context.SaveChangesAsync();
            return order.Id;
        }

        public async Task<Order?> GetByIdAsync(Guid id) =>
            await context.Orders.FindAsync(id);

        public async Task<List<Order>> GetAllAsync(int page, int pageSize) =>
            await context.Orders
                .OrderByDescending(o => o.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
    }
}
