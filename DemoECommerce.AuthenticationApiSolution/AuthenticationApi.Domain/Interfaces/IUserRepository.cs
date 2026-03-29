using AuthenticationApi.Domain.Entities;
using System.Linq.Expressions;

namespace AuthenticationApi.Domain.Interfaces
{
    public interface IUserRepository
    {
        Task<User> CreateAsync(User entity);
        Task<User?> FindByIdAsync(Guid id);
        Task<User?> GetByAsync(Expression<Func<User, bool>> predicate);
    }
}
