using Microsoft.Extensions.Configuration;
using Shatbli.Data.Models;
using Shatbli.Data.Models.Enums;
using Shatbli.Infrustructure.Abstracts;
using Shatbli.Service.Interfaces;

namespace Shatbli.Service.Implementations
{
    public class AdminSeedService : IAdminSeedService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IAuthenticationService _authService;
        private readonly IConfiguration _configuration;

        public AdminSeedService(
            IUnitOfWork unitOfWork,
            IAuthenticationService authService,
            IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _authService = authService;
            _configuration = configuration;
        }

        public async Task SeedAdminUserAsync()
        {
            // Check if admin already exists
            var adminExists = await _unitOfWork.Users.CountAsync(u => u.Role == UserRole.Admin && !u.IsDeleted) > 0;

            if (!adminExists)
            {
                var adminEmail = _configuration["AdminSettings:Email"] ?? "admin@shatbli.com";
                var adminPassword = _configuration["AdminSettings:Password"] ?? "Admin@123456";
                var adminFullName = _configuration["AdminSettings:FullName"] ?? "System Administrator";

                var admin = new User
                {
                    Email = adminEmail,
                    PasswordHash = _authService.HashPassword(adminPassword),
                    FullName = adminFullName,
                    Role = UserRole.Admin,
                    IsActive = true,
                    IsEmailVerified = true,
                    CreatedAt = DateTime.UtcNow,
                    UpdatedAt = DateTime.UtcNow
                };

                await _unitOfWork.Users.AddAsync(admin);
                await _unitOfWork.CompleteAsync();

                Console.WriteLine($"✅ Admin user created successfully!");
                Console.WriteLine($"📧 Email: {adminEmail}");
                Console.WriteLine($"🔑 Password: {adminPassword}");
                Console.WriteLine($"⚠️ Please change the password after first login!");
            }
        }
    }
}