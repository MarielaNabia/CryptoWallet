using CyptoWallet.Entities;
using Microsoft.EntityFrameworkCore;

namespace CyptoWallet.DataAccess.DBSeeding
{
    public class RoleSeeder : IEntitySeeder
    {
        public void SeedDatabase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Role>().HasData(
                new Role
                {
                    RoleId = 1,
                    Name = "Admin",
                    Description = "Administrador",
                    Activo = true,

                },
                 new Role
                 {
                     RoleId = 2,
                     Name = "Consultor",
                     Description = "Consultor",
                     Activo = true,
                 });
        }
    }
}
