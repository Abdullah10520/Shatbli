using Microsoft.EntityFrameworkCore;
using Shatbli.Data.Models;
using Shatbli.Infrustructure.Abstracts;

namespace Shatbli.Infrustructure.Repositories
{
    public class DesignRepository : GenericRepositoryAsync<Design>, IDesignRepository
    {
        public DesignRepository(Context.AppContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Design>> GetUserDesignsAsync(int userId)
        {
            return await _context.Designs
                .Include(d => d.SelectedCeramicProduct)
                .Where(d => d.UserId == userId && !d.IsDeleted)
                .OrderByDescending(d => d.CreatedAt)
                .ToListAsync();
        }

        public async Task<Design?> GetByIdWithDetailsAsync(int designId)
        {
            return await _context.Designs
                .Include(d => d.SelectedCeramicProduct)
                .Include(d => d.User)
                .FirstOrDefaultAsync(d => d.Id == designId && !d.IsDeleted);
        }
    }
}