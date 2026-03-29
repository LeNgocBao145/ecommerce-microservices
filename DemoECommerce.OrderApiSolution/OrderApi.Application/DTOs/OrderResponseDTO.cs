using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record OrderResponseDTO(
        [Required]
        string Id,
        [Required]
        Guid ProductId,
        [Required]
        Guid ClientId,
        [Required, Range(1, int.MaxValue)]
        int PurchaseQuantity
    );
}
