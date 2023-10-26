using CyptoWallet.DataAccess.Repositories.Interfaces;
using CyptoWallet.Entities;
using CyptoWallet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Security.Claims;
using System.Security.Principal;

namespace CyptoWallet.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;

        public AccountController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Obtiene una cuenta por el tipo de cuenta asociada a un usuario.
        /// </summary>
        /// <param name="accountType">El tipo de cuenta a consultar.</param>
        /// <returns>Detalles de la cuenta si se encuentra, de lo contrario, devuelve un código de estado correspondiente.</returns>
        [Authorize(Policy = "AdminConsultor")]       
        [HttpGet("AccountXUser")]
        public async Task<IActionResult> GetAccount(int accountType)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(); // El usuario no está autenticado o el token es inválido
            }

            // cuentas asociadas al usuario.
            var userAccounts = await _unitOfWork.AccountRepository.GetAccountsByUserIdAsync(userId);

            if (userAccounts == null || !userAccounts.Any())
            {
                return NotFound("No se encontraron cuentas para este usuario.");
            }

            // Busca la cuenta especificada por el usuario (accountType)
            var selectedAccount = userAccounts.FirstOrDefault(account => account.AccountTypeId == accountType);

            if (selectedAccount == null)
            {
                return BadRequest($"No se encontró una cuenta de tipo {accountType} para este usuario.");
            }
            return Ok(selectedAccount);
            //var account = await _unitOfWork.AccountRepository.GetByIdAsync(id);
            //if (account == null)
            //{
            //    return NotFound();
            //}
            //return Ok(account);
        }

        /// <summary>
        /// Obtiene el saldo de todas las cuentas asociadas al usuario autenticado.
        /// </summary>
        /// <returns>Saldo de cada tipo de cuenta.</returns>
        [Authorize(Policy = "AdminConsultor")]     
        [HttpGet("balance")]
        public async Task<IActionResult> GetAccountBalance()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(); // El usuario no está autenticado o el token es inválido
            }

            // Ahora userId es un entero y puedes usarlo para buscar cuentas asociadas al usuario.
            var userAccounts = await _unitOfWork.AccountRepository.GetAccountsByUserIdAsync(userId);

            if (userAccounts == null || !userAccounts.Any())
            {
                return NotFound("No se encontraron cuentas para este usuario.");
            }

            // Carga ansiosamente la propiedad AccountType
            foreach (var account in userAccounts)
            {
                await _unitOfWork.AccountRepository.LoadAccountTypeAsync(account);
            }

            // Calcula el saldo de cada tipo de cuenta
            var balances = new Dictionary<string, decimal>();

            foreach (var account in userAccounts)
            {
                var accountTypeName = account.AccountType.Name;
                if (!balances.ContainsKey(accountTypeName))
                {
                    balances[accountTypeName] = 0;
                }
                balances[accountTypeName] += account.Balance;
            }

            return Ok(balances);
        }

        /// <summary>
        /// Realiza un depósito en la cuenta especificada.
        /// </summary>
        /// <param name="identifier">Identificador de la cuenta (CBU, Alias o UUID).</param>
        /// <param name="amount">Monto a depositar.</param>
        /// <returns>Resultado del depósito.</returns>
        [HttpPost("Deposit")]
        public async Task<IActionResult> DepositAsync(string identifier, decimal amount)
        {
            var account = await _unitOfWork.AccountRepository.GetAccountByIdentifierAsync(identifier);

            if (account == null)
            {
                return NotFound();
            }

            if (amount <= 0)
            {
                return BadRequest("El monto del depósito debe ser mayor que cero.");
            }



            account.Balance += amount;
            await _unitOfWork.AccountRepository.UpdateAsync(account);

            return Ok(new
            {
                message = $"Su depósito de {amount:C}, a la cuenta {identifier} se realizó con éxito."
            });

            //return Ok($"Su depósito de {amount}, a la cuenta {identifier} se realizó con éxito.");
        }

        /// <summary>
        /// Realiza un retiro de una cuenta específica.
        /// </summary>
        /// <param name="amount">Monto a retirar.</param>
        /// <param name="accountType">Tipo de cuenta.</param>
        /// <returns>Resultado del retiro.</returns>
        [Authorize(Policy = "AdminConsultor")]       
        [HttpPost("withdraw")]
        public async Task<IActionResult> Withdraw([FromQuery] decimal amount, [FromQuery] int accountType)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
            {
                return Unauthorized(); // El usuario no está autenticado o el token es inválido
            }

            // cuentas asociadas al usuario.
            var userAccounts = await _unitOfWork.AccountRepository.GetAccountsByUserIdAsync(userId);

            if (userAccounts == null || !userAccounts.Any())
            {
                return NotFound("No se encontraron cuentas para este usuario.");
            }

            // Busca la cuenta especificada por el tipo (accountType)
            var selectedAccount = userAccounts.FirstOrDefault(account => account.AccountTypeId == accountType);

            if (selectedAccount == null)
            {
                return BadRequest($"No se encontró una cuenta de tipo {accountType} para este usuario.");
            }

            if (amount <= 0)
            {
                return BadRequest("Monto de retiro no válido.");
            }

            if (amount > selectedAccount.Balance)
            {
                return BadRequest($"Saldo insuficiente en la cuenta de tipo {accountType} para el retiro.");
            }

            selectedAccount.Balance -= amount;
            await _unitOfWork.AccountRepository.UpdateAsync(selectedAccount);

            return Ok(new { message = $"Retiro de {amount:C} en cuenta de tipo {accountType} realizado con éxito. Nuevo saldo: {selectedAccount.Balance:C}" });

            // return Ok($"Retiro de {amount:C} en cuenta de tipo {accountType} realizado con éxito. Nuevo saldo: {selectedAccount.Balance:C}");
        }


    }
}
