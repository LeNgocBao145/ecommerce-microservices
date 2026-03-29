using eCommerce.SharedLibrary.Interfaces;
using OrderApi.Domain.Entities;
using System.Linq.Expressions;

namespace OrderApi.Domain.Interfaces
{
    public interface IOrderRepository : IRepository<Order>
    {
        Task<IEnumerable<Order>> GetOrdersAsync(Expression<Func<Order, bool>> predicate);
    }
}
