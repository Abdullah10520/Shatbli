using Shatbli.Data.DTOs.UserDtos;
using Shatbli.Data.Models;

namespace Shatbli.Service.Interfaces
{
    public interface IUserService
    {
        Task<User> RegisterUserAsync(RegisterUserDto dto);
        Task<AuthResponseDto> LoginUserAsync(LoginUserDto dto);
        Task<UserProfileDto> GetUserProfileAsync(int userId);
    }
}