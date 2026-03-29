using AuthenticationApi.Application.DTOs;
using AuthenticationApi.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthenticationApi.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController(IUserService userService) : ControllerBase
    {
        [HttpGet("{id}")]
        public async Task<ActionResult<UserResponseDTO>> GetUserById(Guid id)
        {
            // Implementation for getting user by ID
            var user = await userService.GetUserById(id);
            if (user == null)
            {
                return NotFound("User not found");
            }
            return Ok(user);
        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register([FromBody] UserRequestDTO registerRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Implementation for user registration
            var user = await userService.Register(registerRequest);
            return user is not null ? Ok(user) : BadRequest(Request);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDTO loginRequest)
        {
            // Implementation for user login
            var token = await userService.Login(loginRequest);
            return Ok(token);
        }
    }
}
