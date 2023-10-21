using CyptoWallet.DataAccess;
using CyptoWallet.DataAccess.Repositories;

namespace CyptoWallet.Services
{
    public class UnitOfWorkService : IUnitOfWork
    {
        private readonly AppDbContext _context;
        public UserRepository UserRepository { get; private set; }
        public RoleRepository RoleRepository { get; private set; }

        
    public UnitOfWorkService(AppDbContext context)
    {
        _context = context;
        UserRepository = new UserRepository(_context);
        RoleRepository = new RoleRepository(_context);        
    }
    public Task<int> Complete()
    {
        return _context.SaveChangesAsync();
    }
    }
}
