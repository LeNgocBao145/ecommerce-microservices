using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Application.DTOs
{
    public record LoginRequestDTO(
        [Required, EmailAddress] string Email,
        [Required, MinLength(6)] string Password
    );
}