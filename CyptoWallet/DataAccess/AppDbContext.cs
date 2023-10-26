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
        public DbSet<Account> Accounts { get; set; }
        public DbSet<Operation> Operations { get; set; }
        public DbSet<OperationType> OperationType { get; set; }

        public DbSet<AccountType> AccountType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>()
                  .HasMany(u => u.Accounts)
                  .WithOne(a => a.User)
                  .HasForeignKey(a => a.UserId)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<User>()
                .HasMany(u => u.Operations)
                .WithOne(o => o.User)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Account>()
                .HasOne(a => a.User)
                .WithMany(u => u.Accounts)
                .HasForeignKey(a => a.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Operation>()
                .HasOne(o => o.User)
                .WithMany(u => u.Operations)
                .HasForeignKey(o => o.UserId)
                .OnDelete(DeleteBehavior.Restrict);


            // Definir otras relaciones y configuraciones aquí si es necesario.



            var seeders = new List<IEntitySeeder>
            {
                new UserSeeder(),
                new RoleSeeder(),
                new AccountTypeSeeder(),
                new OperationTypeSeeder(),

            };

            foreach (var seeder in seeders)
            {

                seeder.SeedDatabase(modelBuilder);
            }

            base.OnModelCreating(modelBuilder);
        }
    }
}

