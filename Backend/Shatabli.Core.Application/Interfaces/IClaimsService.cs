using System.Security.Claims;

namespace Shatabli.Core.Application.Interfaces
{
    public interface IClaimsService
    {
        string GetCurrentUserId();
        //string? GetCurrentUserEmail();
        //string? GetCurrentUserName();
        //IEnumerable<Claim> GetAllClaims();
        //bool IsAuthenticated();
    }
}