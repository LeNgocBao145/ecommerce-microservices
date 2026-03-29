using AuthenticationApi.Domain.Enums;
using System.ComponentModel.DataAnnotations;

namespace AuthenticationApi.Application.DTOs
{
    public record UserRequestDTO(
         [MaxLength(50)] string Username,
         [Required, EmailAddress] string Email,
         [Required, MinLength(6)] string Password,
         [Phone] string PhoneNumber,
         [MaxLength(200)] string Address,
         [Required] Role Role
     );
}
