using CyptoWallet.ApiClient;
using CyptoWallet.DataAccess.Repositories;
using CyptoWallet.DataAccess.Repositories.Interfaces;
using CyptoWallet.DTOs;
using CyptoWallet.Entities;
using CyptoWallet.Services;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace CyptoWallet.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class OperationController : ControllerBase
    {       
        private readonly CriptoApiClient _criptoApiClient;
        private readonly DolarApiClient _dolarApiClient;
        private readonly IUnitOfWork _unitOfWork;

        public OperationController(CriptoApiClient cryptoService, DolarApiClient dollarService, IUnitOfWork unitOfWork)
        {   
            _criptoApiClient = cryptoService;
            _dolarApiClient = dollarService;
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        /// Obtiene los detalles de una operación por su identificador.
        /// </summary>
        /// <param name="id">El identificador de la operación.</param>
        /// <returns>Detalles de la operación en una respuesta HTTP.</returns>
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOperation(int id)
        {
            var operation = await _unitOfWork.OperationRepository.GetByIdAsync(id);
            if (operation == null)
            {
                return NotFound();
            }
            return Ok(operation);
        }

        /// <summary>
        /// Obtiene el historial de operaciones para una cuenta específica.
        /// </summary>
        /// <param name="id">El identificador de la cuenta.</param>
        /// <returns>Historial de operaciones en una respuesta HTTP.</returns>
        [HttpGet("{id}/history")]
        public async Task<IActionResult> GetOperationHistory(int id)
        {
            var operations = await _unitOfWork.OperationRepository.GetOperationsByUserIdAsync(id);

            // Mapea las entidades de operación a la proyección OperationDto
            var operationDtos = operations.Select(operation => new OperationDto
            {
                Timestamp = operation.Timestamp,
                Amount = operation.Amount,
                SourceAccountName = operation.SourceAccount.Alias, // Cambia .Name a .Alias
                DestinationAccountName = operation.DestinationAccount?.Alias, // Cambia .Name a .Alias
                OperationType = operation.OperationType.Name
            }).ToList();

            return Ok(operationDtos);
        }

        /// <summary>
        /// Realiza una transferencia de fondos entre cuentas.
        /// </summary>
        /// <param name="transferRequestDto">Datos de la transferencia.</param>
        /// <returns>Resultado de la transferencia en una respuesta HTTP.</returns>
        [HttpPost("transfer")]
        public async Task<IActionResult> TransferFunds([FromBody] TransferRequestDto transferRequestDto)
        {
            try
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
              
                // Busca las cuentas de origen y destino en función del alias o CBU proporcionado.
                var sourceAccount = await _unitOfWork.AccountRepository.GetAccountByAliasOrCBUAsync(userId, transferRequestDto.SourceAliasOrCBU);
                var destinationAccount = await _unitOfWork.AccountRepository.GetAccountByAliasOrCBUAsync(userId, transferRequestDto.DestinationAliasOrCBU);

                if (sourceAccount == null || destinationAccount == null)
                {
                    return BadRequest("Una de las cuentas no existe.");
                }

                if (sourceAccount.UserId != userId || destinationAccount.UserId != userId)
                {
                    return BadRequest("No puedes transferir fondos entre cuentas de diferentes usuarios.");
                }

                // Verifica el tipo de cuenta de origen y destino.
                if (sourceAccount.AccountType.Name == AccountTypes.Pesos && destinationAccount.AccountType.Name == AccountTypes.Dolares)
                {
                    // La transferencia es de pesos a dólares.
                    // Utiliza el valor de compra segun API para la conversión.
                    var dollarQuote = await _dolarApiClient.GetDollarQuote("bolsa");
                    if (dollarQuote == null)
                    {
                        return BadRequest("No se pudieron obtener las cotizaciones necesarias.");
                    }
                    decimal amountInDollars = transferRequestDto.Amount / dollarQuote.Compra;

                    // Verifica que el saldo de la cuenta de origen sea suficiente para la transferencia.
                    if (sourceAccount.Balance < transferRequestDto.Amount)
                    {
                        return BadRequest("Saldo insuficiente en la cuenta.");
                    }

                    // Resto el monto de pesos de la cuenta de origen y sumo el monto en dólares a la cuenta de destino.
                    sourceAccount.Balance -= transferRequestDto.Amount;
                    destinationAccount.Balance += amountInDollars;
                }
                else if (sourceAccount.AccountType.Name == AccountTypes.Dolares && destinationAccount.AccountType.Name == AccountTypes.Pesos)
                {
                    // La transferencia es de dólares a pesos.
                    // Utiliza el valor de venta segun API para la conversión.
                    var dollarQuote = await _dolarApiClient.GetDollarQuote("bolsa");
                    if (dollarQuote == null)
                    {
                        return BadRequest("No se pudieron obtener las cotizaciones necesarias.");
                    }
                    decimal amountInPesos = transferRequestDto.Amount * dollarQuote.Venta;

                    // Verifica que el saldo de la cuenta de origen sea suficiente para la transferencia.
                    if (sourceAccount.Balance < transferRequestDto.Amount)
                    {
                        return BadRequest("Saldo insuficiente en la cuenta de origen.");
                    }

                    // Resto el monto en dólares de la cuenta de origen y sumo el monto en pesos a la cuenta de destino.
                    sourceAccount.Balance -= transferRequestDto.Amount;
                    destinationAccount.Balance += amountInPesos;
                }
                else
                {
                    return BadRequest("No se pueden transferir fondos.");
                }

                // Actualiza las cuentas en la base de datos.
                await _unitOfWork.AccountRepository.UpdateAsync(sourceAccount);
                await _unitOfWork.AccountRepository.UpdateAsync(destinationAccount);

                // Registra la operación de transferencia.
                // Busca el tipo de operación "Transferencia" en la base de datos (ajusta esto según tu estructura de repositorio).
                var transferOperationType = await _unitOfWork.OperationRepository.GetOperationTypeByNameAsync(OperationType.Types.Transferencia);
                if (transferOperationType == null)
                {                   
                    return BadRequest("Tipo de operación no encontrado.");
                }
                // Asigna el monto negativo a la operación.
                decimal amount = -transferRequestDto.Amount;

                // Registra la operación de transferencia con el tipo de operación obtenido de la base de datos.
                var transferOperation = new Operation
                {
                    Timestamp = DateTime.UtcNow,
                    Amount = amount,
                    OperationType = transferOperationType, 
                    SourceAccountId = sourceAccount.AccountId,
                    DestinationAccountId = destinationAccount.AccountId,
                    UserId = sourceAccount.UserId 
                };

                await _unitOfWork.OperationRepository.CreateAsync(transferOperation);

                return Ok("Transferencia exitosa");
            }
            catch (Exception ex)
            {               
                return StatusCode(500, "Error interno del servidor");
            }
        }

        /// <summary>
        /// Permite a un usuario comprar una cantidad específica de Bitcoin.
        /// </summary>
        /// <param name="amountInDollars">Cantidad en dólares a gastar en Bitcoin.</param>
        /// <returns>Resultado de la compra de Bitcoin en una respuesta HTTP.</returns>
        [HttpPost("buy-bitcoin")]
        public async Task<IActionResult> BuyBitcoin(decimal amountInDollars)
        {
            try
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

                // Busca la cuenta en dólares del usuario.
                var dollarAccount = userAccounts[1];

                if (dollarAccount == null)
                {
                    return BadRequest("No se encontró una cuenta en dólares para este usuario.");
                }

                // Busca la cuenta de Bitcoin del usuario.
                var btcAccount = userAccounts[2];

                if (btcAccount == null)
                {
                    return BadRequest("No se encontró una cuenta de Bitcoin para este usuario.");
                }

                // Verifica que el saldo en dólares sea suficiente para la compra de Bitcoin.
                if (dollarAccount.Balance < amountInDollars)
                {
                    return BadRequest("Saldo insuficiente en la cuenta en dólares.");
                }

                // Obtén la cotización de Bitcoin.
                var bitcoinInfo = await _criptoApiClient.GetCryptocurrencyInfo("1"); // Ajusta el parámetro según tu API

                if (bitcoinInfo == null)
                {
                    return BadRequest("No se pudieron obtener las cotizaciones necesarias.");
                }

                // Calcula la cantidad de Bitcoin que se puede comprar con la cantidad en dólares.
                decimal bitcoinAmount = amountInDollars / bitcoinInfo.PriceUSD;

                // Realiza la operación de compra de Bitcoin y registra la transacción.
                dollarAccount.Balance -= amountInDollars;
                btcAccount.Balance += bitcoinAmount;

                ////*
                // Actualiza las cuentas en la base de datos.
                await _unitOfWork.AccountRepository.UpdateAsync(dollarAccount);
                await _unitOfWork.AccountRepository.UpdateAsync(btcAccount);

                // Registra la operación de transferencia.
                // Busca el tipo de operación "Compra" en la base de datos (ajusta esto según tu estructura de repositorio).
                var purchaseOperationType = await _unitOfWork.OperationRepository.GetOperationTypeByNameAsync(OperationType.Types.Compra);
                if (purchaseOperationType == null)
                {
                    return BadRequest("Tipo de operación no encontrado.");
                }

                // Registra la operación de compra con el tipo de operación obtenido de la base de datos.
                var purchaseOperation = new Operation
                {
                    Timestamp = DateTime.UtcNow,
                    Amount = -amountInDollars, // Amount in dollars spent for Bitcoin
                    OperationType = purchaseOperationType,
                    SourceAccountId = dollarAccount.AccountId,
                    DestinationAccountId = btcAccount.AccountId,
                    UserId = userId
                };

                await _unitOfWork.OperationRepository.CreateAsync(purchaseOperation);

                // Actualiza las cuentas en la base de datos.
                await _unitOfWork.AccountRepository.UpdateAsync(dollarAccount);
                await _unitOfWork.AccountRepository.UpdateAsync(btcAccount);

                return Ok($"Has comprado {bitcoinAmount} BTC.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error en la operación de compra: " + ex.Message);
            }
        }

        /// <summary>
        /// Permite a un usuario vender una cantidad específica de Bitcoin.
        /// </summary>
        /// <param name="bitcoinAmount">Cantidad de Bitcoin a vender.</param>
        /// <returns>Resultado de la venta de Bitcoin en una respuesta HTTP.</returns>
        [HttpPost("sell-bitcoin")]
        public async Task<IActionResult> SellBitcoin(decimal bitcoinAmount)
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);

                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(); // El usuario no está autenticado o el token es inválido
                }

                // Cuentas asociadas al usuario.
                var userAccounts = await _unitOfWork.AccountRepository.GetAccountsByUserIdAsync(userId);

                if (userAccounts == null || userAccounts.Count < 3)
                {
                    return NotFound("No se encontraron cuentas para este usuario.");
                }

                // Busca la cuenta en dólares del usuario.
                var dollarAccount = userAccounts[1];

                if (dollarAccount == null)
                {
                    return BadRequest("No se encontró una cuenta en dólares para este usuario.");
                }

                // Busca la cuenta de Bitcoin del usuario.
                var btcAccount = userAccounts[2];

                if (btcAccount == null)
                {
                    return BadRequest("No se encontró una cuenta de Bitcoin para este usuario.");
                }

                // Verifica que el saldo en Bitcoin sea suficiente para la venta.
                if (btcAccount.Balance < bitcoinAmount)
                {
                    return BadRequest("Saldo insuficiente en la cuenta de Bitcoin.");
                }

                // Obtén la cotización de Bitcoin.
                var bitcoinInfo = await _criptoApiClient.GetCryptocurrencyInfo("1"); // Ajusta el parámetro según tu API

                if (bitcoinInfo == null)
                {
                    return BadRequest("No se pudieron obtener las cotizaciones necesarias.");
                }

                // Calcula la cantidad de dólares que se obtendrán por la venta de Bitcoin.
                decimal amountInDollars = bitcoinAmount * bitcoinInfo.PriceUSD;

                // Realiza la operación de venta de Bitcoin y registra la transacción.
                dollarAccount.Balance += amountInDollars;
                btcAccount.Balance -= bitcoinAmount;

                // Actualiza las cuentas en la base de datos.
                await _unitOfWork.AccountRepository.UpdateAsync(dollarAccount);
                await _unitOfWork.AccountRepository.UpdateAsync(btcAccount);

                // Registra la operación de venta.
                // Busca el tipo de operación "Venta" en la base de datos (ajusta esto según tu estructura de repositorio).
                var sellOperationType = await _unitOfWork.OperationRepository.GetOperationTypeByNameAsync(OperationType.Types.Venta);
                if (sellOperationType == null)
                {
                    return BadRequest("Tipo de operación no encontrado.");
                }

                // Registra la operación de venta con el tipo de operación obtenido de la base de datos.
                var sellOperation = new Operation
                {
                    Timestamp = DateTime.UtcNow,
                    Amount = -bitcoinAmount, // Amount in dollars received for Bitcoin
                    OperationType = sellOperationType,
                    SourceAccountId = btcAccount.AccountId,
                    DestinationAccountId = dollarAccount.AccountId,
                    UserId = userId
                };

                await _unitOfWork.OperationRepository.CreateAsync(sellOperation);

                return Ok($"Has vendido {bitcoinAmount} BTC por {amountInDollars} dólares.");
            }
            catch (Exception ex)
            {
                return BadRequest("Error en la operación de venta: " + ex.Message);
            }
        }
      

    }
}

