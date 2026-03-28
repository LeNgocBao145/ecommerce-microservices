using System.Linq.Expressions;
namespace eCommerce.SharedLibrary.Interfaces
{
    public interface IRepository<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<int> UpdateAsync(T entity);
        Task<int> DeleteAsync(Guid id);
        Task<T> CreateAsync(T entity);
        Task<T?> FindByIdAsync(Guid id);
        Task<T?> GetByAsync(Expression<Func<T, bool>> predicate);
    }
}
