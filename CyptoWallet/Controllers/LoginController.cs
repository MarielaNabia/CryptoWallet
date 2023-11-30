using CyptoWallet.DTOs;
using CyptoWallet.Helper;
using CyptoWallet.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace CyptoWallet.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private JwtTokenHelper _jwtTokenHelper;
        private readonly IUnitOfWork _unitOfWork;
        public LoginController(IUnitOfWork unitOfWork, IConfiguration configuration)
        {
            _unitOfWork = unitOfWork;
            _jwtTokenHelper = new JwtTokenHelper(configuration);
        }

        /// <summary>
        ///  Se loguea el usuario
        /// </summary>
        /// <returns>el token del usuario</returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(AuthUserDto dto)
        {
            try
            {
                Log.Information("Intento de inicio de sesión para el usuario: {Email}", dto.Email);

                var userCredentials = await _unitOfWork.UserRepository.AuthenticateCredentials(dto);
            if (userCredentials is null)
                {
                    Log.Warning("Intento de inicio de sesión fallido para el usuario: {Email}", dto.Email);
                    return Unauthorized("Las credenciales son incorrectas");
                }

                var token = _jwtTokenHelper.CrearToken(userCredentials);

            var user = new UserLoginDto()
            {
                Email = userCredentials.Email,
                Token = token
            };

                Log.Information("Inicio de sesión exitoso para el usuario: {Email}", dto.Email);
                return Ok(user);

        }
            catch (Exception ex)
            {
                Log.Error(ex, "Error al procesar la solicitud de inicio de sesión");
                return StatusCode(500, "Error interno del servidor");
            }
        }
    }
}
