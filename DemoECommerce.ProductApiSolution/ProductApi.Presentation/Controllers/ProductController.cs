using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;

namespace ProductApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class ProductsController(IProductService productService) : ControllerBase
    {
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductResponseDTO>>> GetAllProductAsync()
        {
            var products = await productService.GetAllProductsAsync();

            return !products.Any() ? NotFound("Products do not founded") : Ok(products);
        }

        [HttpGet("/{id:guid}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProductByIdAsync(Guid id)
        {
            var product = await productService.GetProductByIdAsync(id);

            return product is null ? NotFound("Product not found") : Ok(product);
        }

        [HttpPut("/{id:guid}")]
        [Authorize]
        public async Task<ActionResult<ProductResponseDTO>> UpdateProductAsync(Guid id, ProductRequestDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var findedProduct = await productService.GetProductByIdAsync(id);
            if (findedProduct is null)
            {
                return NotFound("Product not found");
            }

            var updatedProduct = await productService.UpdateProductAsync(id, product);

            return updatedProduct is null ? BadRequest("Product update failed") : Ok(updatedProduct);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductResponseDTO>> CreateProductAsync(ProductRequestDTO product)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var createdProduct = await productService.CreateProductAsync(product);

            return createdProduct is null ? BadRequest("Create product failed") : Ok(createdProduct);
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<ActionResult> DeleteProductAsync(Guid id)
        {
            int result = await productService.DeleteProductAsync(id);
            return result == 0 ? BadRequest("Product delete failed") : Ok();
        }
    }
}
