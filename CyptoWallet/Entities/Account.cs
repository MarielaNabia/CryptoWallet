using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyptoWallet.Entities
{
    public class Account
    {
        public int AccountId { get; set; }

        [Required]
        [Column(TypeName = "decimal(18, 2)")]
        public decimal Balance { get; set; }      

        // Propiedades para cuentas fiduciarias (pesos y usd)
        [MaxLength(100)]
        public string CBU { get; set; } 

        [MaxLength(100)]
        public string Alias { get; set; }

        // Propiedad para cuentas de cripto UUID
        [MaxLength(36)] 
        public string CryptoAddress { get; set; }

        // Relación con el usuario propietario de la cuenta
        public int UserId { get; set; }
        public User User { get; set; }

        // Relación con el tipo de cuenta
        public int AccountTypeId { get; set; }
        public AccountType AccountType { get; set; }
    }
}
