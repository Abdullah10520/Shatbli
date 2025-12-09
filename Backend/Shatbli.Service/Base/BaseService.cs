using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Logging;

namespace Shatbli.Service.Base
{
    public abstract class BaseService
    {
        protected async Task<T> ExecuteWithTransactionAsync<T>(
            Func<Task<T>> operation,
            Func<Task<IDbContextTransaction>> getTransactionAsync,
            ILogger logger = null)
        {
            await using var transaction = await getTransactionAsync();
            try
            {
                logger?.LogInformation("Transaction started: {TransactionId}", transaction.TransactionId);

                var result = await operation();

                await transaction.CommitAsync();
                logger?.LogInformation("Transaction committed successfully: {TransactionId}", transaction.TransactionId);

                return result;
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Transaction failed and rolled back: {TransactionId}", transaction.TransactionId);
                await transaction.RollbackAsync();
                throw;
            }
        }

        protected async Task ExecuteWithTransactionAsync(
            Func<Task> operation,
            Func<Task<IDbContextTransaction>> getTransactionAsync,
            ILogger logger = null)
        {
            await using var transaction = await getTransactionAsync();
            try
            {
                logger?.LogInformation("Transaction started: {TransactionId}", transaction.TransactionId);

                await operation();

                await transaction.CommitAsync();
                logger?.LogInformation("Transaction committed successfully: {TransactionId}", transaction.TransactionId);
            }
            catch (Exception ex)
            {
                logger?.LogError(ex, "Transaction failed and rolled back: {TransactionId}", transaction.TransactionId);
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}