using Microsoft.EntityFrameworkCore;
using Shatbli.Data.Models;
using Shatbli.Data.Models.Enums;
using Shatbli.Infrustructure.Abstracts;

namespace Shatbli.Infrustructure.Repositories
{
    public class ProductRepository : GenericRepositoryAsync<Product>, IProductRepository
    {
        public ProductRepository(Context.AppContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Product>> GetByCategoryAsync(ProductCategory category, bool activeOnly = true)
        {
            var query = _context.Products.Where(p => p.Category == category && !p.IsDeleted);

            if (activeOnly)
                query = query.Where(p => p.IsActive);

            return await query.OrderByDescending(p => p.CreatedAt).ToListAsync();
        }

        public async Task<IEnumerable<Product>> GetActiveProductsAsync()
        {
            return await _context.Products
                .Where(p => p.IsActive && !p.IsDeleted)
                .OrderByDescending(p => p.CreatedAt)
                .ToListAsync();
        }
    }
}