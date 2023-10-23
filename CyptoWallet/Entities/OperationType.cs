using System.ComponentModel.DataAnnotations;

namespace CyptoWallet.Entities
{
    public class OperationType
    {
        public int OperationTypeId { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }       

        // Relación con las operaciones
        public ICollection<Operation> Operations { get; set; }

        public static class Types
        {
            public const string Transferencia = "Transferencia";
            public const string Compra = "Compra";
            public const string Deposito = "Depósito";
            public const string Venta = "Venta";
            public const string Retiro = "Retiro";
            // Agrega más tipos de transacciones 
        }
    }
   

}
