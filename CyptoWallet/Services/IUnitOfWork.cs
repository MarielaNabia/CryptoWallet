using CyptoWallet.DataAccess.Repositories;

namespace CyptoWallet.Services
{
    public interface IUnitOfWork
    {
        public UserRepository UserRepository { get; }
        public RoleRepository RoleRepository { get; }
        Task<int> Complete();
    }
}
