using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;

namespace Shatabli.Core.Application.Features.Users.Commands.UpdateProfile
{
    public class UpdateProfileCommandHandler : IRequestHandler<UpdateProfileCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IClaimsService _claimsService;

        public UpdateProfileCommandHandler(IApplicationDbContext context, IClaimsService claimsService)
        {
            _context = context;
            _claimsService = claimsService;
        }

        public async Task<Result> Handle(UpdateProfileCommand request, CancellationToken cancellationToken)
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

            user.FullName = request.FullName;
            user.PhoneNumber = request.PhoneNumber;

            await _context.SaveChangesAsync(cancellationToken);

            return Result.Success("Profile updated successfully");
        }
    }
}