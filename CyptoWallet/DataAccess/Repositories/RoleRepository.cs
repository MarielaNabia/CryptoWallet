using CyptoWallet.DataAccess.Repositories.Interfaces;
using CyptoWallet.Entities;
using Microsoft.EntityFrameworkCore;

namespace CyptoWallet.DataAccess.Repositories
{
    public class RoleRepository : Repository<Role>, IRoleRepository
    {
        public RoleRepository(AppDbContext context) : base(context)
        {

        }

        public override async Task<bool> UpdateAsync(Role updateRole)
        {
            var Role = await _context.Roles.FirstOrDefaultAsync(x => x.RoleId == updateRole.RoleId);
            if (Role == null) { return false; }

            Role.Name = updateRole.Name;
            Role.Description = updateRole.Description;
            Role.Activo = updateRole.Activo;

            _context.Roles.Update(Role);
            return true;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            var Role = await _context.Roles.Where(x => x.RoleId == id).FirstOrDefaultAsync();
            if (Role != null)
            {
                _context.Roles.Remove(Role);
            }

            return true;
        }
    }
}
