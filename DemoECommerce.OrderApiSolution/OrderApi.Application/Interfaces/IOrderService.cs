using OrderApi.Application.DTOs;

namespace OrderApi.Application.Interfaces
{
    public interface IOrderService
    {
        Task<IEnumerable<OrderResponseDTO>> GetOrdersByClientId(Guid clientId);
        Task<OrderDetailsDTO> GetOrderDetails(Guid orderId);
    }
}
