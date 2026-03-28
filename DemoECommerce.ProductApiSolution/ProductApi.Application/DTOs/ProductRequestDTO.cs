using System.ComponentModel.DataAnnotations;

namespace ProductApi.Application.DTOs
{
    public class ProductRequestDTO
    {
        [Required, MaxLength(255)]
        public string Name { get; set; }
        [Required, DataType(DataType.Currency)]
        public decimal Price { get; set; }
        [Required, Range(0, int.MaxValue)]
        public int Quantity { get; set; }
    }
}
