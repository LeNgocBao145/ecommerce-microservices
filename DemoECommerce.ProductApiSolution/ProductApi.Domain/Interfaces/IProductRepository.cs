using eCommerce.SharedLibrary.Interfaces;
using ProductApi.Domain.Entities;

namespace ProductApi.Application.Interfaces
{
    public interface IProductRepository : IRepository<Product> { }
}
