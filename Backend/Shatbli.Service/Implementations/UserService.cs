using Shatbli.Data.DTOs.UserDtos;
using Shatbli.Data.Models;
using Shatbli.Data.Models.Enums;
using Shatbli.Infrustructure.Abstracts;
using Shatbli.Service.Interfaces;

namespace Shatbli.Service.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authService;

        public UserService(IUnitOfWork unitOfWork, IAuthenticationService authService)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
        }

        public async Task<User> RegisterUserAsync(RegisterUserDto dto)
        {
            // Check if email exists
            if (await _unitOfWork.Users.IsEmailExistsAsync(dto.Email))
            {
                throw new Exception("Email already exists");
            }

            // Create user
            var user = new User
            {
                Email = dto.Email,
                PasswordHash = _authService.HashPassword(dto.Password),
                FullName = dto.FullName,
                PhoneNumber = dto.PhoneNumber,
                Role = UserRole.Homeowner,
                IsActive = true,
                IsEmailVerified = false
            };

            await _unitOfWork.Users.AddAsync(user);
            await _unitOfWork.CompleteAsync();

            return user;
        }

        public async Task<AuthResponseDto> LoginUserAsync(LoginUserDto dto)
        {
            var user = await _unitOfWork.Users.GetByEmailAsync(dto.Email);

            if (user == null || !_authService.VerifyPassword(dto.Password, user.PasswordHash))
            {
                throw new UnauthorizedAccessException("Invalid email or password");
            }

            if (!user.IsActive)
            {
                throw new UnauthorizedAccessException("Account is deactivated");
            }

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            _unitOfWork.Users.Update(user);
            await _unitOfWork.CompleteAsync();

            // Generate token
            var token = _authService.GenerateJwtToken(user.Id, user.Email, user.Role.ToString());

            return new AuthResponseDto
            {
                Token = token,
                ExpiresAt = DateTime.UtcNow.AddDays(7),
                User = new UserProfileDto
                {
                    Id = user.Id,
                    Email = user.Email,
                    FullName = user.FullName,
                    PhoneNumber = user.PhoneNumber,
                    Role = user.Role,
                    CreatedAt = user.CreatedAt,
                    LastLoginAt = user.LastLoginAt,
                    IsEmailVerified = user.IsEmailVerified,
                    TotalDesigns = user.Designs.Count
                }
            };
        }

        public async Task<UserProfileDto> GetUserProfileAsync(int userId)
        {
            var user = await _unitOfWork.Users.GetByIdWithDesignsAsync(userId);

            if (user == null)
            {
                throw new KeyNotFoundException("User not found");
            }

            return new UserProfileDto
            {
                Id = user.Id,
                Email = user.Email,
                FullName = user.FullName,
                PhoneNumber = user.PhoneNumber,
                Role = user.Role,
                CreatedAt = user.CreatedAt,
                LastLoginAt = user.LastLoginAt,
                IsEmailVerified = user.IsEmailVerified,
                TotalDesigns = user.Designs.Count
            };
        }
    }
}