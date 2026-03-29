using System.ComponentModel.DataAnnotations;

namespace ProductApi.Application.DTOs
{
    public record ProductRequestDTO
    (
        [Required, MaxLength(255)]
        string Name,
        [Required, DataType(DataType.Currency)]
        decimal Price,
        [Required, Range(0, int.MaxValue)]
        int Quantity
    );
}
