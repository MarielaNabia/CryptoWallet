using CyptoWallet.DTOs;
using CyptoWallet.Entities;
using CyptoWallet.Helper;
using CyptoWallet.Infraestructure;
using CyptoWallet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CyptoWallet.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        /// <summary>
        ///  Devuelve todo los usuarios
        /// </summary>
        /// <returns>retorna todos los usuarios</returns>
        [Authorize(Policy = "AdminConsultor")]
        [HttpGet]
        [Route("GetAll")]
        public async Task<IActionResult> GetAll()
        {
            var users = await _unitOfWork.UserRepository.GetAllAsync();
            var usersDto = users.Select(j => new RegisterDto
            {
                Email = j.Email,
                Nombre = j.Nombre,
                Apellido = j.Apellido,
                DNI = j.DNI,
                Password = j.Password,
                RoleId = j.RoleId
            }).ToList();
            int pageToShow = 1;

            if (Request.Query.ContainsKey("page")) int.TryParse(Request.Query["page"], out pageToShow);

            var url = new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}").ToString();

            var paginateUsers = PaginateHelper.Paginate(usersDto, pageToShow, url);

            return ResponseFactory.CreateSuccessResponse(200, paginateUsers);
        }

        /// <summary>
        /// Obtiene un usuario por su ID.
        /// </summary>
        /// <param name="id">El ID del usuario a obtener.</param>
        /// <returns>El usuario encontrado.</returns>
        [Authorize(Policy = "AdminConsultor")]
        [HttpGet("GetById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var user = await _unitOfWork.UserRepository.GetByIdAsync(id);

            if (user == null)
            {
                return ResponseFactory.CreateErrorResponse(404, $"Usuario con ID {id} no encontrado.");
            }

            return ResponseFactory.CreateSuccessResponse(200, user);
        }

        /// <summary>
        ///  Registra el usuario
        /// </summary>
        /// <param name="dto"></param>
        /// <returns>devuelve un usuario registrado con un statusCode 201</returns>

        [Authorize(Policy = "AdminConsultor")]
        [HttpPost]
        [Route("Alta")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {

            if (await _unitOfWork.UserRepository.UserEx(dto.Email)) return ResponseFactory.CreateErrorResponse(409, $"Ya existe un usuario registrado con el usuario:{dto.Email}");
            var user = new User(dto);
            await _unitOfWork.UserRepository.CreateAsync(user);
            await _unitOfWork.Complete();

            // Crear cuentas para el usuario
            var userId = user.UserId; // Obtén el ID del usuario recién registrado

            // Crear cuentas con valores predeterminados
            var accountPesos = CreateAccount(userId, 1, "Pesos");
            var accountUSD = CreateAccount(userId, 2, "USD");
            var accountBTC = CreateAccount(userId, 3, "BTC");

            // Almacenar las cuentas en la base de datos
            await _unitOfWork.AccountRepository.CreateAsync(accountPesos);
            await _unitOfWork.AccountRepository.CreateAsync(accountUSD);
            await _unitOfWork.AccountRepository.CreateAsync(accountBTC);
            await _unitOfWork.Complete();

            return ResponseFactory.CreateSuccessResponse(201, "Usuario registrado con exito!");
        }

        /// <summary>
        ///  Actualiza el usuario
        /// </summary>
        /// <returns>actualizado o un 500</returns>
        [Authorize(Policy = "AdminConsultor")]
        [HttpPut("Modificar/{id}")]
        public async Task<IActionResult> Update([FromRoute] int id, RegisterDto dto)
        {

            var result = await _unitOfWork.UserRepository.UpdateAsync(new User(dto, id));

            if (!result)
            {
                return ResponseFactory.CreateErrorResponse(500, "No se pudo modificar el usuario");
            }
            else
            {
                await _unitOfWork.Complete();
                return ResponseFactory.CreateSuccessResponse(200, "ok.Actualizado");
            }
        }


        /// <summary>
        ///  Elimina el usuario
        /// </summary>
        /// <returns>Eliminado o un 500</returns>
        [Authorize(Policy = "AdminConsultor")]
        [HttpPut("Baja/{id}")]
        public async Task<IActionResult> Delete([FromRoute] int id)
        {
            var result = await _unitOfWork.UserRepository.DeleteAsync(id);

            if (!result)
            {
                return ResponseFactory.CreateErrorResponse(500, "No se pudo eliminar el usuario");
            }
            else
            {
                await _unitOfWork.Complete();
                return ResponseFactory.CreateSuccessResponse(200, "Eliminado");
            }
        }

        private Account CreateAccount(int userId, int accountTypeId, string currency)
        {
            string cbuValue = accountTypeId == 3 ? "NoCorresponde" : $"CBU{currency}";
            string aliasValue = accountTypeId == 3 ? "NoCorresponde" : $"Alias{currency}";

            return new Account
            {
                Balance = 0.0m, // Saldo inicial
                CBU = cbuValue, // Valor predeterminado para CBU
                Alias = aliasValue, // Valor predeterminado para Alias
                CryptoAddress = $"CryptoAddress{currency}", // Valor predeterminado para CryptoAddress
                AccountTypeId = accountTypeId, // Asigna el AccountTypeId
                UserId = userId, // Asigna el UserId del usuario recién registrado
            };
        }
    }
}
