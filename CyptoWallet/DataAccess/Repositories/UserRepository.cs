using CyptoWallet.DataAccess.Repositories.Interfaces;
using CyptoWallet.DTOs;
using CyptoWallet.Entities;
using CyptoWallet.Helper;
using Microsoft.EntityFrameworkCore;

namespace CyptoWallet.DataAccess.Repositories
{
    public class UserRepository : Repository<User>, IUserRepository
    {
        public UserRepository(AppDbContext context) : base(context)
        {
        }

        public override async Task<bool> UpdateAsync(User updateUser)
        {
            var user = await _context.Users.FirstOrDefaultAsync(x => x.UserId == updateUser.UserId);
            if (user == null) { return false; }

            user.Email = updateUser.Email;
            user.Nombre = updateUser.Nombre;
            user.Apellido = updateUser.Apellido;
            user.DNI = updateUser.DNI;
            user.RoleId = updateUser.RoleId;
            user.Activo = updateUser.Activo;
            user.Password = PassEncryptHelper.CreatePass(updateUser.Password, updateUser.Email);

            _context.Users.Update(user);
            return true;
        }

        public override async Task<bool> DeleteAsync(int id)
        {
            var user = await _context.Users.Where(x => x.UserId == id).FirstOrDefaultAsync();
            if (user != null)
            {
                user.Activo = false;
                _context.Users.Update(user);
            }

            return true;
        }

        public async Task<User?> AuthenticateCredentials(AuthUserDto dto)
        {
            string encryptedPassword = PassEncryptHelper.CreatePass(dto.Password, dto.Email);

            return await _context.Users
                .Include(x => x.Role)
                .SingleOrDefaultAsync(x => x.Email == dto.Email && x.Password == encryptedPassword);
            // return await _context.Users.Include(x => x.Role).SingleOrDefaultAsync(x => x.CodUsuario == dto.CodUsuario && x.Password == PassEncryptHelper.CreatePass(dto.Password, dto.CodUsuario));
        }

        public async Task<bool> UserEx(string email)
        {
            return await _context.Users.AnyAsync(x => x.Email == email);
        }
    }
}
