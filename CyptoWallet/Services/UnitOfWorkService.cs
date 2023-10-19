using CyptoWallet.DataAccess;
using Microsoft.EntityFrameworkCore;

namespace CyptoWallet.Services
{
    public class UnitOfWorkService : IUnitOfWork
    {
        private readonly AppDbContext _context;
    }

}
