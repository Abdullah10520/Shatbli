using Shatbli.Infrustructure.Abstracts;

namespace Shatbli.Infrustructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly Context.AppContext _context;

        public IUserRepository Users { get; private set; }
        public IProductRepository Products { get; private set; }
        public IDesignRepository Designs { get; private set; }
        public IAIProcessingLogRepository AIProcessingLogs { get; private set; }

        public UnitOfWork(Context.AppContext context)
        {
            _context = context;
            Users = new UserRepository(context);
            Products = new ProductRepository(context);
            Designs = new DesignRepository(context);
            AIProcessingLogs = new AIProcessingLogRepository(context);
        }

        public async Task<int> CompleteAsync()
        {
            return await _context.SaveChangesAsync();
        }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}