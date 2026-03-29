using AuthenticationApi.Domain.Enums;

namespace AuthenticationApi.Application.DTOs
{
    public record UserResponseDTO(
        Guid Id,
        string Username,
        string Email,
        string PhoneNumber,
        string Address,
        Role Role,
        DateTime CreatedAt
    );
}