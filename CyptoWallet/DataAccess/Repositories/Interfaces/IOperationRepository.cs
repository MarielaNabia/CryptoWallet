using CyptoWallet.Entities;

namespace CyptoWallet.DataAccess.Repositories.Interfaces
{
    public interface IOperationRepository : IRepository<Operation>
    {
        Task<List<Operation>> GetTransactionsForUserAsync(int userId);
        Task<List<Operation>> GetOperationsByAccountIdAsync(int accountId);
        Task<OperationType> GetOperationTypeByNameAsync(string operationTypeName);
    }
}
