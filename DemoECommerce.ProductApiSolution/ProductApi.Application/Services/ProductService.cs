using AutoMapper;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Domain.Entities;
using ProductApi.Domain.Exceptions;

namespace ProductApi.Application.Services
{
    public class ProductService(IProductRepository productRepository, IMapper mapper) : IProductService
    {
        public async Task<ProductResponseDTO> CreateProductAsync(ProductRequestDTO product)
        {
            var createdProduct = await productRepository.CreateAsync(mapper.Map<Product>(product));
            return mapper.Map<ProductResponseDTO>(createdProduct);
        }

        public async Task DeleteProductAsync(Guid id)
        {
            if (productRepository.FindByIdAsync(id) == null)
            {
                throw new ProductNotFoundException("Product with this id does not exists");
            }
            await productRepository.DeleteAsync(id);
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetAllProductsAsync()
        {
            var products = await productRepository.GetAllAsync();
            return products.Select(p => mapper.Map<ProductResponseDTO>(p));
        }

        public async Task<ProductResponseDTO> GetProductByIdAsync(Guid id)
        {
            var product = await productRepository.FindByIdAsync(id);
            return mapper.Map<ProductResponseDTO>(product);
        }

        public async Task<ProductResponseDTO> UpdateProductAsync(Guid id, ProductRequestDTO product)
        {
            var convertedProduct = mapper.Map<Product>(product);
            var updatedProduct = await productRepository.UpdateAsync(convertedProduct);
            return mapper.Map<ProductResponseDTO>(updatedProduct);
        }
    }
}
