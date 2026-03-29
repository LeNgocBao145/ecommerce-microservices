using AuthenticationApi.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace AuthenticationApi.Infrastructure.Persistence
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users { get; set; }
    }
}
