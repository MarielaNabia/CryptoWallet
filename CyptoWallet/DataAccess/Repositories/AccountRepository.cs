using CyptoWallet.DataAccess.Repositories.Interfaces;
using CyptoWallet.Entities;
using Microsoft.EntityFrameworkCore;

namespace CyptoWallet.DataAccess.Repositories
{
    public class AccountRepository : IRepository<Account>
    {
        private readonly AppDbContext _context;

        public AccountRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Account> GetByIdAsync(int accountId)
        {
            return await _context.Accounts.FindAsync(accountId);          
        }         
        
        public Task<List<Account>> GetAllAsync()
        {
            return _context.Accounts.ToListAsync();
        }

        public async Task<bool> CreateAsync(Account entity)
        {
            _context.Accounts.Add(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> UpdateAsync(Account entity)
        {
            _context.Accounts.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var account = await GetByIdAsync(id);
            if (account != null)
            {
                _context.Accounts.Remove(account);
                await _context.SaveChangesAsync();
                return true;
            }
            return false;
        }

        public async Task<Account> GetAccountByIdentifierAsync(string identifier)
        {        

            // Verifica el tipo de cuenta y busca la cuenta correspondiente
            if (identifier.Length == 32) // UUID de cripto
            {
                return await _context.Accounts.SingleOrDefaultAsync(a => a.CryptoAddress == identifier );
            }
            else if (identifier.Length == 22) // CBU
            {
                return await _context.Accounts.SingleOrDefaultAsync(a => a.CBU == identifier );
            }
            else // Alias
            {
                return _context.Accounts.SingleOrDefault(a => a.Alias == identifier );
            }
        }
        public async Task<List<Account>> GetAccountsByUserIdAsync(int userId)
        {
            return await _context.Accounts.Where(a => a.UserId == userId).ToListAsync();
        }
        public async Task<Account> GetAccountByAliasOrCBUAsync(int userId, string aliasOrCBU)       
         {
            return await _context.Accounts
            .Include(account => account.AccountType) // Carga la propiedad de navegación AccountType
            .Where(account => account.UserId == userId && (account.Alias == aliasOrCBU || account.CBU == aliasOrCBU))
            .FirstOrDefaultAsync();
        }

         public async Task LoadAccountTypeAsync(Account account)
        {
            // Incluye la propiedad AccountType al cargar la cuenta
            await _context.Entry(account)
                .Reference(a => a.AccountType)
                .LoadAsync();
        }

    }
}
