using CyptoWallet.DataAccess.Repositories.Interfaces;
using CyptoWallet.DTOs;
using CyptoWallet.Entities;
using CyptoWallet.Helper;
using CyptoWallet.Services;
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

        public async Task<Account?> CreateAccount(int userId, int accountTypeId, string currency)
        {
            string cbuValue = accountTypeId == 3 ? "NoCorresponde" : GenerateUniqueCBU();
            string aliasValue = accountTypeId == 3 ? "NoCorresponde" : GenerateUniqueAlias();
            string cryptoAddress = (accountTypeId == 1 || accountTypeId == 2) ? "NoCorresponde" : GenerateUniqueCryptoAddress();

            return new Account
            {
                Balance = 0.0m, // Saldo inicial
                CBU = cbuValue, // Valor generado o "NoCorresponde"
                Alias = aliasValue, // Valor generado o "NoCorresponde"
                CryptoAddress = cryptoAddress, // Valor generado o "NoCorresponde"
                AccountTypeId = accountTypeId, // Asigna el AccountTypeId
                UserId = userId, // Asigna el UserId del usuario recién registrado
            };
        }


        private string GenerateUniqueCBU()
        {
            string cbu;
            bool isUnique;

            do
            {
                // Genera un CBU único con "20" o "21" seguido de 18 caracteres al azar
                cbu = GenerateRandomCBUWithPrefix();

                // Verifica si el CBU ya existe en la base de datos
                isUnique = !_context.Accounts.Any(account => account.CBU == cbu);

            } while (!isUnique);

            return cbu;
        }
        private string GenerateRandomCBUWithPrefix()
        {
            var random = new Random();
            // Genera un número aleatorio entre 0 y 1 para determinar si el CBU comienza con "20" o "21"
            var prefix = (random.Next(2) == 0) ? "20" : "21";
            // Genera 18 caracteres al azar
            var characters = "0123456789";
            var cbu = prefix + new string(Enumerable.Repeat(characters, 20).Select(s => s[random.Next(s.Length)]).ToArray());
            return cbu;
        }

        private string GenerateUniqueAlias()
        {
            string alias;
            bool isUnique;

            do
            {
                // Genera un Alias único
                alias = GenerateRandomAlias();

                // Verifica si el Alias ya existe en la base de datos
                isUnique = !_context.Accounts.Any(account => account.Alias == alias);

            } while (!isUnique);

            return alias;
        }

        private string GenerateRandomAlias()
        {
            string[] words = { "PIEDRA", "PAPEL", "TIJERA", "AGUA", "FUEGO", "AIRE", "TIERRA", "SOL", "LUNA", "ESTRELLA", "MAR", "MONTAÑA", "BOSQUE" };
            var random = new Random();

            // Elige tres palabras al azar de la lista
            var randomWords = words.OrderBy(x => random.Next()).Take(3);
            var alias = string.Join(".", randomWords);

            return alias;
        }

        private string GenerateUniqueCryptoAddress()
        {
            string cryptoAddress;
            bool isUnique;

            do
            {
                // Genera un CryptoAddress único
                cryptoAddress = GenerateRandomCryptoAddress();

                // Verifica si el CryptoAddress ya existe en la base de datos
                isUnique = !_context.Accounts.Any(account => account.CryptoAddress == cryptoAddress);

            } while (!isUnique);

            return cryptoAddress;
        }

        private string GenerateRandomCryptoAddress()
        {
            // Genera un CryptoAddress en formato UUID (sin guiones)
            return Guid.NewGuid().ToString("N");
        }
    }
    }
