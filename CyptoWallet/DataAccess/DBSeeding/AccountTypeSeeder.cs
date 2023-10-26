using CyptoWallet.Entities;
using Microsoft.EntityFrameworkCore;

namespace CyptoWallet.DataAccess.DBSeeding
{
    public class AccountTypeSeeder : IEntitySeeder
    {
        public void SeedDatabase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<AccountType>().HasData(
                 new AccountType
                 {
                     AccountTypeId = 1, 
                     Name = AccountTypes.Pesos
                 },
                new AccountType
                {
                    AccountTypeId = 2,  
                    Name = AccountTypes.Dolares
                },
                new AccountType
                {
                    AccountTypeId = 3,  
                    Name = AccountTypes.BTC
                });
        }
    }
}

