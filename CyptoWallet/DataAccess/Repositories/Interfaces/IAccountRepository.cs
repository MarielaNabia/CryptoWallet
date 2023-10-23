using CyptoWallet.Entities;

namespace CyptoWallet.DataAccess.Repositories.Interfaces
{
    public interface IAccountRepository : IRepository<Account>
    {
        Task<Account> GetByIdAsync(int accountId);
        Task<decimal> GetBalanceAsync(int accountId);
        Task<bool> DepositAsync(int accountId, decimal amount);
        Task<bool> WithdrawAsync(int accountId, decimal amount);
    }
}
