using OrderApi.Domain.Enums;

namespace OrderApi.Application.DTOs
{
    public record UserDTO(
        Guid Id,
        string Username,
        string Email,
        string PhoneNumber,
        string Address,
        Role Role
    );
}
