using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Shatabli.Core.Application.Interfaces;
using Shatabli.Core.Domain.Entities;
using Shatabli.Core.Domain.Enums;

namespace Shatabli.Core.Application.Features.Users.Commands.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, string>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;

        public RegisterCommandHandler(IApplicationDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<string> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            // Check if email already exists
            var existingUser = await _context.Users
                .FirstOrDefaultAsync(u => u.Email == request.Email, cancellationToken);

            if (existingUser != null)
            {
                throw new InvalidOperationException("Email already exists");
            }

            // Hash password (you should use a proper hashing library like BCrypt)
            var passwordHash = HashPassword(request.Password);

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

            return user.Id;
        }

        private string HashPassword(string password)
        {
            // TODO: Implement proper password hashing using BCrypt.Net-Next
            // For now, returning a placeholder
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}