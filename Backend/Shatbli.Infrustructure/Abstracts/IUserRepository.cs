using Shatbli.Data.Models;

namespace Shatbli.Infrustructure.Abstracts
{
    public interface IUserRepository : IGenericRepositoryAsync<User>
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdWithDesignsAsync(int id);
        Task<bool> IsEmailExistsAsync(string email);
    }
}