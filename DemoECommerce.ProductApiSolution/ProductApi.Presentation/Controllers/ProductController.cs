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
            return Ok(products);
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<ProductResponseDTO>> GetProductByIdAsync(Guid id)
        {
            var product = await productService.GetProductByIdAsync(id);
            return Ok(product);
        }

        [HttpPut("{id:guid}")]
        [Authorize]
        public async Task<ActionResult<ProductResponseDTO>> UpdateProductAsync(Guid id, ProductRequestDTO product)
        {
            var products = await productService.UpdateProductAsync(id, product);
            return Ok(products);
        }

        [HttpPost]
        [Authorize]
        public async Task<ActionResult<ProductResponseDTO>> CreateProductAsync(ProductRequestDTO product)
        {
            var products = await productService.CreateProductAsync(product);
            return Ok(products);
        }

        [HttpDelete("{id:guid}")]
        [Authorize]
        public async Task<ActionResult> DeleteProductAsync(Guid id)
        {
            await productService.DeleteProductAsync(id);
            return Ok();
        }
    }
}
