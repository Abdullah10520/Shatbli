namespace Shatbli.Service.Interfaces
{
    public interface IAuthenticationService
    {
        string HashPassword(string password);
        bool VerifyPassword(string password, string hashedPassword);
        string GenerateJwtToken(int userId, string email, string role);
    }
}