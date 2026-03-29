using AuthenticationApi.Domain.Enums;

namespace AuthenticationApi.Domain.Entities
{
    public class User
    {
        public Guid Id { get; set; }
        public string? Username { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public string? Address { get; set; }
        public Role? Role { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
