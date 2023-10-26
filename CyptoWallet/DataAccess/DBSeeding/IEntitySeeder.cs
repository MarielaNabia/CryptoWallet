using Microsoft.EntityFrameworkCore;

namespace CyptoWallet.DataAccess.DBSeeding
{
    public interface IEntitySeeder
    {
        void SeedDatabase(ModelBuilder modelBuilder);
    }
}
