using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Data;
using CyptoWallet.DTOs;
using CyptoWallet.Helper;

namespace CyptoWallet.Entities
{
    public class User
    {
        public User()
        {
            Activo = true;
        }

        public User(RegisterDto dto)
        {
            Email = dto.Email;
            Nombre = dto.Nombre;
            Apellido = dto.Apellido;
            DNI = dto.DNI;
            RoleId = dto.RoleId;
            Password = PassEncryptHelper.CreatePass(dto.Password, dto.Email);
            Activo = true;
        }


        public User(RegisterDto dto, int id)
        {
            UserId = id;
            Email = dto.Email;
            Nombre = dto.Nombre;
            Apellido = dto.Apellido;
            DNI = dto.DNI;
            RoleId = dto.RoleId;
            Password = PassEncryptHelper.CreatePass(dto.Password, dto.Email);
            Activo = true;
        }

        [Key]
        public int UserId { get; set; }
        public string Email { get; set; }
        [Required]
        [StringLength(100)]
        public string Nombre { get; set; }
        [Required]
        [StringLength(100)]
        public string Apellido { get; set; }
        [Required]
        public int DNI { get; set; }
        [Required]
        [Column(TypeName = "VARCHAR(250)")]
        public string Password { get; set; }
        public bool Activo { get; set; }
        public DateTime FechaBaja { get; set; }
        [Required]
        public int RoleId { get; set; }

        [ForeignKey("RoleId")]
        public Role? Role { get; set; }

        public List<Account> Accounts { get; set; }
        public List<Operation> Operations { get; set; }
    }
}

