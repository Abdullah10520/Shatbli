using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;

namespace Shatabli.Core.Application.Features.Users.Commands.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginResponse>>
    {
        private readonly IApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public LoginCommandHandler(IApplicationDbContext context, ITokenService tokenService)
        {
            _context = context;
            _tokenService = tokenService;
        }

        public async Task<Result<LoginResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var user = await _context.Users
                .AsTracking()
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            // ✅ Business validation - NO exception
            if (user == null || !VerifyPassword(request.Password, user.PasswordHash))
            {
                return Result<LoginResponse>.Unauthorized("Invalid email or password");
            }

            if (!user.IsActive)
            {
                return Result<LoginResponse>.Unauthorized("User account is inactive");
            }

            // Update last login
            user.LastLoginAt = DateTime.UtcNow;
            await _context.SaveChangesAsync(cancellationToken);

            // Generate JWT token
            var token = _tokenService.GenerateJwtToken(user);

            var response = new LoginResponse
            {
                Email = user.Email,
                FullName = user.FullName,
                Token = token,
                Role = user.Role.ToString()
            };

            return Result<LoginResponse>.Success(response, "Login successful");
        }

        private bool VerifyPassword(string password, string passwordHash)
        {
            return BCrypt.Net.BCrypt.Verify(password, passwordHash);
        }
    }
}