using Shatbli.Data.Models;
using Shatbli.Data.Models.Enums;

namespace Shatbli.Infrustructure.Abstracts
{
    public interface IProductRepository : IGenericRepositoryAsync<Product>
    {
        Task<IEnumerable<Product>> GetByCategoryAsync(ProductCategory category, bool activeOnly = true);
        Task<IEnumerable<Product>> GetActiveProductsAsync();
    }
}