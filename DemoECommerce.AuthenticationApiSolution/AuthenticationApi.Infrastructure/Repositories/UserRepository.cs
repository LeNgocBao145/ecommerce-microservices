using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Domain.Interfaces;
using AuthenticationApi.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace AuthenticationApi.Infrastructure.Repositories
{
    public class UserRepository(UserDbContext context) : IUserRepository
    {
        public async Task<User> CreateAsync(User entity)
        {
            var user = await context.Users.AddAsync(entity);
            await context.SaveChangesAsync();
            return user.Entity;
        }

        public async Task<User?> FindByIdAsync(Guid id)
        {
            return await context.Users.FindAsync(id);
        }

        public async Task<User?> GetByAsync(Expression<Func<User, bool>> predicate)
        {
            return await context.Users.FirstOrDefaultAsync(predicate);
        }
    }
}
