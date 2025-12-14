using Shatabli.Core.Domain.Entities;

namespace Shatabli.Core.Application.Interfaces
{
    public interface ITokenService
    {
        string GenerateJwtToken(User user);
        int? ValidateJwtToken(string token);
    }
}