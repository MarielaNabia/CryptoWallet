using Microsoft.EntityFrameworkCore;

namespace CyptoWallet.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    }
}

