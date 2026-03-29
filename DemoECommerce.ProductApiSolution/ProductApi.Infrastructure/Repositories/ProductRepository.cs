using Microsoft.EntityFrameworkCore;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Infrastructure.Data;
using System.Linq.Expressions;

namespace ProductApi.Infrastructure.Repositories
{
    public class ProductRepository(ProductDbContext context) : IProductRepository
    {
        public async Task<Product> CreateAsync(Product entity)
        {
            var product = await context.Products.AddAsync(entity);
            await context.SaveChangesAsync();
            return product.Entity;
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            return await context.Products.Where(p => p.Id == id)
                .ExecuteDeleteAsync();
        }

        public async Task<Product?> FindByIdAsync(Guid id)
        {
            return await context.Products.FindAsync(id);
        }

        public async Task<IEnumerable<Product>> GetAllAsync()
        {
            return await context.Products.AsNoTracking().ToListAsync();
        }

        public async Task<Product?> GetByAsync(Expression<Func<Product, bool>> predicate)
        {
            return await context.Products.Where(predicate).AsNoTracking().FirstOrDefaultAsync();
        }

        public async Task<Product?> UpdateAsync(Product entity)
        {
            //var product = FindByIdAsync(entity.Id);
            //context.Entry(product).State = EntityState.Detached;
            //context.Products.Update(entity);
            //return await context.SaveChangesAsync();            

            var rowsAffected = await context.Products
                .Where(product => product.Id == entity.Id)
                .ExecuteUpdateAsync(product => product
                    .SetProperty(p => p.Name, entity.Name)
                    .SetProperty(p => p.Quantity, entity.Quantity)
                    .SetProperty(p => p.Price, entity.Price)
                );

            return rowsAffected > 0 ? entity : null;
        }
    }
}
