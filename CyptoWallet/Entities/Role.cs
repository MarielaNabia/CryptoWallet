using CyptoWallet.DTOs;
using System.ComponentModel.DataAnnotations.Schema;

namespace CyptoWallet.Entities
{
    public class Role
    {
        public int RoleId { get; set; }
        
        public string Name { get; set; }
        
        public string Description { get; set; }
        
        public bool Activo { get; set; }

        public Role()
        {

        }
        public Role(RoleDto dto)
        {
            Name = dto.Name;
            Description = dto.Description;
            Activo = dto.Activo;
        }
    }
}
