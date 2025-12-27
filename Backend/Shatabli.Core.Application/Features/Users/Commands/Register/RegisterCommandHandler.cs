using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Common;
using Shatabli.Core.Domain.Entities;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Application.Features.Users.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Result> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // ✅ Business validation - NO exception
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (existingUser != null)
            {
                return Result.Conflict($"User with email '{request.Email}' already exists");
            }

            // Hash password
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);

            var user = new User
            {
                Id = Guid.NewGuid().ToString(),
                Email = request.Email,
                PasswordHash = passwordHash,
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Role = UserRole.Homeowner,
                IsActive = true,
                IsEmailVerified = false,
                CreatedAt = DateTime.UtcNow
            };

            await _context.Users.AddAsync(user, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);

            await SubscribeUserToFreePlan(user.Id, cancellationToken);

            return Result.Success("User registered successfully");
        }

        private async Task SubscribeUserToFreePlan(string userId, CancellationToken cancellationToken)
        {

            // ✅ Auto-subscribe to Free plan
            var freePlan = await _context.SubscriptionPlans
                .FirstOrDefaultAsync(p => p.Type == PlanType.Free, cancellationToken);

            if (freePlan != null)
            {
                var freeSubscription = new UserSubscription
                {
                    UserId = userId,
                    SubscriptionPlanId = freePlan.Id,
                    StartDate = DateTime.UtcNow,
                    IsActive = true,
                    ImagesGeneratedThisMonth = 0,
                    ImagesGeneratedToday = 0,
                    LastResetDate = DateTime.UtcNow
                };

                await _context.UserSubscriptions.AddAsync(freeSubscription, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);
            }
        }
    }
}