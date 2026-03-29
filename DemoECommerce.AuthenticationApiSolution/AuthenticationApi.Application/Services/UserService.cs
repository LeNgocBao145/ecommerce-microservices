using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using AuthenticationApi.Domain.Entities;
using AuthenticationApi.Domain.Exceptions;
using AuthenticationApi.Domain.Interfaces;
using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthenticationApi.Application.Services
{
    public class UserService(IUserRepository userRepository, IConfiguration config, IMapper mapper) : IUserService
    {
        public async Task<UserResponseDTO> GetUserById(Guid id)
        {
            var user = await userRepository.FindByIdAsync(id);
            if (user == null)
            {
                throw new UserNotFoundException("User not found");
            }
            return mapper.Map<User, UserResponseDTO>(user);
        }

        public async Task<LoginResponseDTO> Login(LoginRequestDTO loginRequest)
        {
            var user = await userRepository.GetByAsync(u => u.Email == loginRequest.Email);
            if (user == null)
            {
                throw new UserNotFoundException($"Invalid login attempt for email {loginRequest.Email}");
            }

            bool verified = BCrypt.Net.BCrypt.Verify(loginRequest.Password, user!.Password);
            if (!verified)
            {
                throw new InvalidCredentialsException($"Invalid credentials");
            }

            // Assuming you have a method to generate a JWT token
            var token = GenerateJwtToken(user);

            return new LoginResponseDTO(token);
        }

        private string GenerateJwtToken(User user)
        {
            var secretKey = Encoding.UTF8.GetBytes(config.GetSection("Authentication:Key").Value!);
            var securityKey = new SymmetricSecurityKey(secretKey);
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, user.Id.ToString())
            };

            var token = new JwtSecurityToken(
                issuer: config.GetSection("Authentication:Issuer").Value,
                audience: config.GetSection("Authentication:Audience").Value,
                claims: claims,
                expires: DateTime.Now.AddHours(1),
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        public async Task<UserResponseDTO> Register(UserRequestDTO user)
        {
            var existingUser = await userRepository.GetByAsync(u => u.Email == user.Email);
            if (existingUser != null)
            {
                throw new EmailAlreadyExistsException($"User with this email {user.Email} already exists");
            }

            var convertedUser = mapper.Map<UserRequestDTO, User>(user);
            convertedUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var createdUser = await userRepository.CreateAsync(convertedUser);
            return mapper.Map<User, UserResponseDTO>(createdUser);
        }
    }
}
