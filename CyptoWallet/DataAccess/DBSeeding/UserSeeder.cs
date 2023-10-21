using CyptoWallet.Entities;
using CyptoWallet.Helper;
using Microsoft.EntityFrameworkCore;

namespace CyptoWallet.DataAccess.DBSeeding
{
    public class UserSeeder : IEntitySeeder
    {
        public void SeedDatabase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = 1,
                    Email = "tecno@gmail.com",
                    Nombre = "Carlos",
                    Apellido = "Marx",
                    DNI = 40123456,
                    Password = PassEncryptHelper.CreatePass("123456", "tecno@gmail.com"),
                    RoleId = 1
                });
        }
    }
}
