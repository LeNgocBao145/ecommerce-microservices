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
            if (createdProduct is null)
            {
                throw new ProductOperationException("Failed to create the product");
            }
            return mapper.Map<ProductResponseDTO>(createdProduct);
        }

        public async Task<int> DeleteProductAsync(Guid id)
        {
            var product = await productRepository.FindByIdAsync(id);
            if (product is null)
            {
                throw new ProductNotFoundException("Product with this id does not exist");
            }
            int isSuccess = await productRepository.DeleteAsync(id);
            if (isSuccess == 0)
            {
                throw new ProductOperationException("Failed to delete the product");
            }
            return isSuccess;
        }

        public async Task<IEnumerable<ProductResponseDTO>> GetAllProductsAsync()
        {
            var products = await productRepository.GetAllAsync();
            return products.Select(p => mapper.Map<ProductResponseDTO>(p));
        }

        public async Task<ProductResponseDTO> GetProductByIdAsync(Guid id)
        {
            var product = await productRepository.FindByIdAsync(id);
            if (product is null)
            {
                throw new ProductNotFoundException($"Product with id '{id}' does not exist");
            }
            return mapper.Map<ProductResponseDTO>(product);
        }

        public async Task<ProductResponseDTO> UpdateProductAsync(Guid id, ProductRequestDTO product)
        {
            var foundedProduct = await productRepository.FindByIdAsync(id);
            if (foundedProduct is null)
            {
                throw new ProductNotFoundException($"Product with id '{id}' does not exist");
            }
            var convertedProduct = mapper.Map<Product>(product);
            convertedProduct.Id = id;
            var updatedProduct = await productRepository.UpdateAsync(convertedProduct);
            if (updatedProduct is null)
            {
                throw new ProductOperationException("Failed to update the product");
            }
            return mapper.Map<ProductResponseDTO>(updatedProduct);
        }
    }
}
