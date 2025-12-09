using Shatbli.Data.Models.Enums;

namespace Shatbli.Data.Models
{
    public class User : BaseEntity
    {
        public string Email { get; set; } = string.Empty;
        public string PasswordHash { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public UserRole Role { get; set; } = UserRole.Homeowner;
        
        // Navigation properties
        public ICollection<Design> Designs { get; set; } = new List<Design>();
        
        // Metadata
        public DateTime? LastLoginAt { get; set; }
        public bool IsActive { get; set; } = true;
        public bool IsEmailVerified { get; set; } = false;
    }
}