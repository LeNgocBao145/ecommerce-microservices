using AutoMapper;
using OrderApi.Application.DTOs;
using OrderApi.Application.Interfaces;
using OrderApi.Domain.Interfaces;
using Polly.Registry;
using System.Net.Http.Json;

namespace OrderApi.Application.Services
{
    public class OrderService(IOrderRepository orderRepository,
        HttpClient client, ResiliencePipelineProvider<string> resiliencePipeline, IMapper mapper) : IOrderService
    {
        public async Task<ProductDTO> GetProduct(Guid productId)
        {
            // Call Product API using HttpClient
            // Redirect this call to API Gateway since product api does not respond to to outsiders
            var getProduct = await client.GetAsync($"/api/products/{productId}");
            if (!getProduct.IsSuccessStatusCode)
            {
                return null!;
            }

            var product = await getProduct.Content.ReadFromJsonAsync<ProductDTO>();
            return product;
        }
        public async Task<OrderDetailsDTO> GetOrderDetails(Guid orderId)
        {
            var order = await orderRepository.FindByIdAsync(orderId);
            if (order is null)
            {
                return null!;
            }

            // Get Retry Pipeline from Polly registry
            var retryPipeline = resiliencePipeline.GetPipeline("my-retry-pipeline");

            var productDTO = await retryPipeline.ExecuteAsync(async token => await GetProduct(order.ProductId));

            var userDTO = await retryPipeline.ExecuteAsync(async token => await GetUser(order.ClientId));

            return new OrderDetailsDTO(
                orderId,
                productDTO.Id,
                userDTO.Id,
                userDTO.Email,
                userDTO.PhoneNumber,
                productDTO.Name,
                order.PurchaseQuantity,
                productDTO.Price,
                productDTO.Price * order.PurchaseQuantity,
                order.OrderedDate
                );
        }

        private async Task<UserDTO> GetUser(Guid clientId)
        {
            var getUser = await client.GetAsync($"/api/users/{clientId}");
            if (!getUser.IsSuccessStatusCode)
            {
                return null!;
            }

            var user = await getUser.Content.ReadFromJsonAsync<UserDTO>();
            return user;
        }

        public async Task<IEnumerable<OrderResponseDTO>> GetOrdersByClientId(Guid clientId)
        {
            var orders = await orderRepository.GetOrdersAsync(o => o.ClientId == clientId);
            if (!orders.Any())
            {
                return null!;
            }

            return orders.Select(o => mapper.Map<OrderResponseDTO>(o));
        }
    }
}
