namespace Shatbli.Data.DTOs.UserDtos
{
    public class RegisterUserDto
    {
        public string Email { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
        public string FullName { get; set; }
        public string? PhoneNumber { get; set; }
    }
}