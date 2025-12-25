using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;

namespace Shatabli.Core.Application.Features.Users.Commands.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IClaimsService _claimsService;

        public ChangePasswordCommandHandler(IApplicationDbContext context, IClaimsService claimsService)
        {
            _context = context;
            _claimsService = claimsService;
        }

        public async Task<Result> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var requestUserId = _claimsService.GetCurrentUserId();
            var user = await _context.Users
                .AsTracking()
                .FirstOrDefaultAsync(u => u.Id == requestUserId, cancellationToken);

            // ✅ Business validation - NO exception
            if (user == null)
            {
                return Result.NotFound($"User with ID '{requestUserId}' was not found");
            }

            // Verify current password
            if (!BCrypt.Net.BCrypt.Verify(request.CurrentPassword, user.PasswordHash))
            {
                return Result.Unauthorized("Current password is incorrect");
            }

            // Hash new password
            user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.NewPassword);

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("Password changed successfully");
        }
    }
}