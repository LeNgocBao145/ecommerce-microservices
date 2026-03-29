using System.ComponentModel.DataAnnotations;

namespace OrderApi.Application.DTOs
{
    public record OrderDetailsDTO(
        [Required]
        Guid OrderId,
        [Required]
        Guid ProductId,
        [Required]
        Guid ClientId,
        [Required, EmailAddress]
        string Email,
        [Required]
        string PhoneNumber,
        [Required]
        string ProductName,
        [Required]
        int PurchaseQuantity,
        [Required, DataType(DataType.Currency)] decimal UnitPrice,
        [Required, DataType(DataType.Currency)] decimal TotalPrice,
        [Required] DateTime OrderedDate
    );
}
