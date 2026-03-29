using AuthenticationApi.Application.DTOs;

namespace AuthenticationApi.Application.Interfaces
{
    public interface IUserService
    {
        Task<UserResponseDTO> Register(UserRequestDTO user);
        Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest);
        Task<UserResponseDTO> GetUserById(Guid id);
    }
}
