using Shatbli.Data.Models;

namespace Shatbli.Infrustructure.Abstracts
{
    public interface IAIProcessingLogRepository : IGenericRepositoryAsync<AIProcessingLog>
    {
        Task<IEnumerable<AIProcessingLog>> GetByDesignIdAsync(int designId);
    }
}