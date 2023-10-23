using CyptoWallet.Entities;
using Microsoft.EntityFrameworkCore;
using static CyptoWallet.Entities.OperationType;

namespace CyptoWallet.DataAccess.DBSeeding
{
    public class OperationTypeSeeder : IEntitySeeder
    {
        public void SeedDatabase(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<OperationType>().HasData(
                new OperationType
                {
                    OperationTypeId = 1,  
                    Name = Types.Transferencia
                },
                new OperationType
                {
                    OperationTypeId = 2,  
                    Name = Types.Compra
                },
                new OperationType
                {
                    OperationTypeId = 3,  
                    Name = Types.Deposito
                },
                new OperationType
                {
                    OperationTypeId = 4,  
                    Name = Types.Venta
                }
               
            );
        }
    }
}
