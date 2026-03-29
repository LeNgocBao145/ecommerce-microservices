using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record CreateOrderDTO(
        [Required]
        Guid ProductId,
        [Required]
        Guid ClientId,
        [Required, Range(1, int.MaxValue, ErrorMessage = "Purchase quantity must be at least 1")]
        int PurchaseQuantity
    );
}
