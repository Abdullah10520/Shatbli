using Microsoft.EntityFrameworkCore;
using Shatbli.Data.Models;
using Shatbli.Infrustructure.Abstracts;

namespace Shatbli.Infrustructure.Repositories
{
    public class AIProcessingLogRepository : GenericRepositoryAsync<AIProcessingLog>, IAIProcessingLogRepository
    {
        public AIProcessingLogRepository(Context.AppContext context) : base(context)
        {
        }

        public async Task<IEnumerable<AIProcessingLog>> GetByDesignIdAsync(int designId)
        {
            return await _context.AIProcessingLogs
                .Where(log => log.DesignId == designId)
                .OrderByDescending(log => log.RequestedAt)
                .ToListAsync();
        }
    }
}