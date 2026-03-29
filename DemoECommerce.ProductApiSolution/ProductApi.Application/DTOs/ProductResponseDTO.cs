namespace ProductApi.Application.DTOs
{
    public record ProductResponseDTO
    (
        Guid Id,
        string Name,
        decimal Price,
        int Quantity
    );
}
