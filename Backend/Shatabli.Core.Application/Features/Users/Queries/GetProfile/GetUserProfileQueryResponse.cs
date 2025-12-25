namespace Shatabli.Core.Application.Features.Users.Queries.GetProfile
{
    public class GetUserProfileQueryResponse
    {
        public string Email { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string? PhoneNumber { get; set; }
        public string Role { get; set; } = string.Empty;
        public DateTime? LastLoginAt { get; set; }
        public bool IsEmailVerified { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}