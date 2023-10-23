using CyptoWallet.ApiClient;
using CyptoWallet.DataAccess;
using CyptoWallet.DataAccess.Repositories;

namespace CyptoWallet.Services
{
    public class UnitOfWorkService : IUnitOfWork
    {
        private readonly AppDbContext _context;       

        public UserRepository UserRepository { get; private set; }
        public RoleRepository RoleRepository { get; private set; }
        public AccountRepository AccountRepository { get; private set; }
        public OperationRepository OperationRepository { get; private set; }


        public UnitOfWorkService(AppDbContext context, CriptoApiClient cryptoService, DolarApiClient dollarService)
        {
        _context = context;
        UserRepository = new UserRepository(_context);
        RoleRepository = new RoleRepository(_context);
        AccountRepository = new AccountRepository(_context);
        OperationRepository = new OperationRepository(_context, cryptoService, dollarService);
        }
    public Task<int> Complete()
    {
        return _context.SaveChangesAsync();
    }
    }
}
