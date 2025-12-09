using Microsoft.EntityFrameworkCore;
using Shatbli.Data.Models;
using Shatbli.Infrustructure.Abstracts;

namespace Shatbli.Infrustructure.Repositories
{
    public class UserRepository : GenericRepositoryAsync<User>, IUserRepository
    {
        public UserRepository(Context.AppContext context) : base(context)
        {
        }

        public async Task<User?> GetByEmailAsync(string email)
        {
            return await _context.Users
                .FirstOrDefaultAsync(u => u.Email == email && !u.IsDeleted);
        }

        public async Task<User?> GetByIdWithDesignsAsync(int id)
        {
            return await _context.Users
                .Include(u => u.Designs)
                .FirstOrDefaultAsync(u => u.Id == id && !u.IsDeleted);
        }

        public async Task<bool> IsEmailExistsAsync(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email && !u.IsDeleted);
        }

        public Task UpdateAsync(User user)
        {
            throw new NotImplementedException();
        }
    }
}