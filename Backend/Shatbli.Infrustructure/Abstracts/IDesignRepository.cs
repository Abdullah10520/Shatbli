using Shatbli.Data.Models;

namespace Shatbli.Infrustructure.Abstracts
{
    public interface IDesignRepository : IGenericRepositoryAsync<Design>
    {
        Task<IEnumerable<Design>> GetUserDesignsAsync(int userId);
        Task<Design?> GetByIdWithDetailsAsync(int designId);
    }
}