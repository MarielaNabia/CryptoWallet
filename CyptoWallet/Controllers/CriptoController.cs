using System.Threading.Tasks;
using CyptoWallet.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CyptoWallet.Controllers
{
    [Route("api/cripto")]
    [ApiController]
    public class CriptoController : ControllerBase
    {
        private readonly CriptoApiClient _apiClient;

        public CriptoController(CriptoApiClient apiClient)
        {
            _apiClient = apiClient;
        }

        /// <summary>
        /// Obtiene información detallada de una criptomoneda por su identificador.
        /// </summary>
        /// <param name="cryptocurrencyId">El identificador único de la criptomoneda.</param>
        /// <returns>
        /// Una respuesta HTTP que contiene la información de la criptomoneda si se encuentra,
        /// un mensaje de "No se encontraron datos para la criptomoneda" si no se encuentra información,
        /// o un mensaje de error en caso de excepción.
        /// </returns>
        [HttpGet("get-cryptocurrency/{cryptocurrencyId}")]
        public async Task<IActionResult> GetCryptocurrencyInfo(string cryptocurrencyId)
        {
            try
            {
                // Llama a tu servicio para obtener la información de la criptomoneda
                var cryptoDto = await _apiClient.GetCryptocurrencyInfo(cryptocurrencyId);

                if (cryptoDto != null)
                {
                    return Ok(cryptoDto);
                }

                return NotFound("No se encontraron datos para la criptomoneda.");
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }
    }
}
