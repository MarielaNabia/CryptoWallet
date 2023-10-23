using System.ComponentModel.DataAnnotations.Schema;
using System.Security.Principal;

namespace CyptoWallet.Entities
{
    public class Operation
    {
        public int OperationId { get; set; }
        public DateTime Timestamp { get; set; }

        [Column(TypeName = "decimal(18, 2)")]
        public decimal Amount { get; set; }

        // Relación con el usuario propietario de la operación
        public int UserId { get; set; }
        public User User { get; set; }

        // Tipo de Operación (Transferencia, Compra, Depósito, etc.)
        public int OperationTypeId { get; set; }
        public OperationType OperationType { get; set; }

        // Cuenta de Origen
        public int SourceAccountId { get; set; }
        public Account SourceAccount { get; set; }

        // Cuenta de Destino (para Transferencias)
        public int? DestinationAccountId { get; set; }
        public Account DestinationAccount { get; set; }

        // Puedes agregar propiedades adicionales según tus necesidades

       
    }
}
