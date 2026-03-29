using OrderApi.Domain.Entities;
using OrderApi.Domain.Interfaces;
using OrderApi.Infrastructure.Persistence;
using System.Linq.Expressions;

namespace OrderApi.Infrastructure.Repositories
{
    public class OrderRepository(OrderDbContext context) : IOrderRepository
    {
        public async Task<Order> CreateAsync(Order entity)
        {
            await context.Orders.AddAsync(entity);
            await context.SaveChangesAsync();
            return entity;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var order = await context.Orders.FindAsync(id);
            if (order is null)
                return 0;

            context.Orders.Remove(order);
            return await context.SaveChangesAsync();
        }

        public async Task<Order?> FindByIdAsync(Guid id)
        {
            return await context.Orders.FindAsync(id);
        }

        public async Task<IEnumerable<Order>> GetAllAsync()
        {
            return await Task.FromResult(context.Orders.AsEnumerable());
        }

        public async Task<Order?> GetByAsync(Expression<Func<Order, bool>> predicate)
        {
            return await Task.FromResult(context.Orders.FirstOrDefault(predicate));
        }

        public async Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate)
        {
            return await Task.FromResult(context.Orders.Where(predicate).AsEnumerable());
        }

        public async Task<Order?> UpdateAsync(Order entity)
        {
            var existingOrder = await context.Orders.FindAsync(entity.Id);
            if (existingOrder is null)
                return null;

            existingOrder.ProductId = entity.ProductId;
            existingOrder.ClientId = entity.ClientId;
            existingOrder.PurchaseQuantity = entity.PurchaseQuantity;
            existingOrder.OrderedDate = entity.OrderedDate;

            context.Orders.Update(existingOrder);
            await context.SaveChangesAsync();
            return existingOrder;
        }
    }
}
