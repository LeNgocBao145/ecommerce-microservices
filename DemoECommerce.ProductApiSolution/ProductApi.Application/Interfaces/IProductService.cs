using ProductApi.Application.DTOs;

namespace ProductApi.Application.Interfaces
{
    public interface IProductService
    {
        Task<IEnumerable<ProductResponseDTO>> GetAllProductsAsync();
        Task<ProductResponseDTO> GetProductByIdAsync(Guid id);
        Task DeleteProductAsync(Guid id);
        Task<ProductResponseDTO> UpdateProductAsync(Guid id, ProductRequestDTO product);
        Task<ProductResponseDTO> CreateProductAsync(ProductRequestDTO product);
    }
}
