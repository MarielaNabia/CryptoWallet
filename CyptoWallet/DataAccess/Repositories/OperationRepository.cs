using CyptoWallet.ApiClient;
using CyptoWallet.DataAccess.Repositories.Interfaces;
using CyptoWallet.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CyptoWallet.DataAccess.Repositories
{
    public class OperationRepository : IRepository<Operation>
    {
        private readonly AppDbContext _context;
        private readonly CriptoApiClient _criptoApiClient;
        private readonly DolarApiClient _dolarApiClient;

        public OperationRepository(AppDbContext context, CriptoApiClient cryptoService, DolarApiClient dollarService)
        {
            _context = context;
            _criptoApiClient = cryptoService;
            _dolarApiClient = dollarService;
        }

        public async Task<List<Operation>> GetAllAsync()
        {
            return await _context.Operations.Include(o => o.OperationType).ToListAsync();
        }

        public async Task<Operation> GetByIdAsync(int id)
        {
            //return await _context.Operations.Include(o => o.OperationId).FirstOrDefaultAsync(o => o.OperationId == id);
            return await _context.Operations.FindAsync(id);
        }

        public async Task<bool> CreateAsync(Operation operation)
        {
            try
            {
                _context.Operations.Add(operation);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> UpdateAsync(Operation operation)
        {
            try
            {
                _context.Entry(operation).State = EntityState.Modified;
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                var operation = await _context.Operations.FindAsync(id);
                if (operation != null)
                {
                    _context.Operations.Remove(operation);
                    await _context.SaveChangesAsync();
                    return true;
                }
                return false;
            }
            catch (Exception)
            {
                return false;
            }
        }

        public async Task<OperationType> GetOperationTypeByNameAsync(string operationTypeName)
        {
            return await _context.OperationType
                .FirstOrDefaultAsync(ot => ot.Name == operationTypeName);
        }

        public async Task<List<Operation>> GetOperationsByUserIdAsync(int userId)
        {
            return await _context.Operations
        .Where(o => o.UserId == userId)
        .Include(o => o.OperationType) 
        .Include(o => o.SourceAccount) 
            .ThenInclude(account => account.AccountType) 
        .Include(o => o.DestinationAccount) 
            .ThenInclude(account => account.AccountType) 
        .ToListAsync();
        }

    }
}
