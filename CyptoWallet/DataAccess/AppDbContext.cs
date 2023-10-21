using CyptoWallet.DataAccess.DBSeeding;
using CyptoWallet.Entities;
using Microsoft.EntityFrameworkCore;

namespace CyptoWallet.DataAccess
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
        public DbSet<User> Users { get; set; }
        public DbSet<Role> Roles { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

            var seeders = new List<IEntitySeeder>
            {
                new UserSeeder(),
                new RoleSeeder(),                

            };

            foreach (var seeder in seeders)
            {

                seeder.SeedDatabase(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}

