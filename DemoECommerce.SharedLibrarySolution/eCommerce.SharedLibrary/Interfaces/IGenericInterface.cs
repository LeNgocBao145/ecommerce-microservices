using eCommerce.SharedLibrary.Responses;
using System.Linq.Expressions;
namespace eCommerce.SharedLibrary.Interfaces
{
    public interface IGenericInterface<T> where T : class
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<Response> UpdateAsync(T entity);
        Task<Response> DeleteAsync(T entity);
        Task<Response> CreateAsync(T entity);
        Task<T> FindByIdAsync(int id);
        Task<T> GetByAsync(Expression<Func<T, bool>> predicate);
    }
}
