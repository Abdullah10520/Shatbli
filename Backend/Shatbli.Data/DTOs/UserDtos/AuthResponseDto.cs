namespace Shatbli.Data.DTOs.UserDtos
{
    public class AuthResponseDto
    {
        public string Token { get; set; }
        public UserProfileDto User { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}