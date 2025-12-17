using Microsoft.AspNetCore.Http;
using Shatabli.Core.Application.Interfaces;
using System.Security.Claims;


namespace Shatabli.Infrastructure.Services
{
    public class ClaimsService : IClaimsService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClaimsService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public string GetCurrentUserId()
        {
            var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            
            if (string.IsNullOrEmpty(userId))
            {
                throw new UnauthorizedAccessException("User is not authenticated or UserId claim is missing");
            }

            return userId;
        }

        //public string? GetCurrentUserEmail()
        //{
        //    return _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.Email);
        //}

        //public string? GetCurrentUserName()
        //{
        //    return _httpContextAccessor.HttpContext?.User?.FindFirstValue(ClaimTypes.Name);
        //}

        //public IEnumerable<Claim> GetAllClaims()
        //{
        //    return _httpContextAccessor.HttpContext?.User?.Claims ?? Enumerable.Empty<Claim>();
        //}

        //public bool IsAuthenticated()
        //{
        //    return _httpContextAccessor.HttpContext?.User?.Identity?.IsAuthenticated ?? false;
        //}
    }
}