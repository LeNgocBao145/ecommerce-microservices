using FakeItEasy;
using FluentAssertions;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductApi.Application.DTOs;
using ProductApi.Application.Interfaces;
using ProductApi.Presentation.Controllers;

namespace UnitTest.ProductApi.Controllers
{
    public class ProductControllerTest
    {
        private readonly IProductService productService;
        private readonly ProductsController productsController;

        public ProductControllerTest()
        {
            // Set up dependencies
            productService = A.Fake<IProductService>();

            //Set up System Under Test (SUT)
            productsController = new ProductsController(productService);
        }

        [Fact]
        public async Task GetProducts_WhenProductsExist_ReturnsOkResult()
        {
            // Arrange
            var products = new List<ProductResponseDTO>
            {
                new() { Id = Guid.NewGuid(), Name = "Product 1", Price = 10.0m },
                new() { Id = Guid.NewGuid(), Name = "Product 2", Price = 20.0m }
            };
            A.CallTo(() => productService.GetAllProductsAsync()).Returns(products);

            // Act
            var result = await productsController.GetAllProductAsync();

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult.StatusCode.Should().Be(StatusCodes.Status200OK);

            var returnedProducts = okResult.Value as IEnumerable<ProductResponseDTO>;
            returnedProducts.Should().NotBeNull();
            returnedProducts.Should().HaveCount(products.Count);
            returnedProducts.First().Id.Should().Be(products.First().Id);
            returnedProducts.Last().Id.Should().Be(products.Last().Id);
        }
    }
}
