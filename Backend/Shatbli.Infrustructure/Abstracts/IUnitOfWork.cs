namespace Shatbli.Infrustructure.Abstracts
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IProductRepository Products { get; }
        IDesignRepository Designs { get; }
        IAIProcessingLogRepository AIProcessingLogs { get; }

        Task<int> CompleteAsync();
        int Complete();
    }
}