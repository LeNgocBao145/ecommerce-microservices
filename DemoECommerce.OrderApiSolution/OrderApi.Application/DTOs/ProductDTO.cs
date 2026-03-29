namespace OrderApi.Application.DTOs
{
    public record ProductDTO(
        Guid Id,
        string Name,
        decimal Price,
        int Quantity
    );
}