using CyptoWallet.DataAccess.Repositories;

namespace CyptoWallet.Services
{
    public interface IUnitOfWork
    {
        public UserRepository UserRepository { get; }
        public RoleRepository RoleRepository { get; }
        public AccountRepository AccountRepository { get; }
        public OperationRepository OperationRepository { get; }

        Task<int> Complete();
    }
}
