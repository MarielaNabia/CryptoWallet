using System;
using System.Threading.Tasks;
using CyptoWallet.ApiClient;
using CyptoWallet.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace CyptoWallet.Controllers
{
    [Route("api/dolar")]
    [ApiController]
    public class DolarController : ControllerBase
    {
        private readonly DolarApiClient _dolarApiClient;

        public DolarController(DolarApiClient dolarApiClient)
        {
            _dolarApiClient = dolarApiClient;
        }

        /// <summary>
        /// Obtiene datos de cotización del dólar para una casa de cambio específica.
        /// </summary>
        /// <param name="casa">El nombre o identificador de la casa de cambio de interés.</param>
        /// <returns>
        /// Una respuesta HTTP que contiene los datos de cotización del dólar para la casa de cambio especificada si se encuentran,
        /// un mensaje de "No se encontraron datos para la casa de cambio especificada" si no se encuentra información,
        /// o un mensaje de error en caso de excepción.
        /// </returns>
        [HttpGet("get-dolar-data/{casa}")]
        public async Task<IActionResult> GetDolarDataByCasa(string casa)
        {
            try
            {
                // Llama al servicio para obtener los datos del dólar para una casa específica
                var dolarInfo = await _dolarApiClient.GetDollarQuote(casa);

                if (dolarInfo != null)
                {
                    // Devuelve los datos del dólar para la casa especificada
                    return Ok(dolarInfo);
                }
                else
                {
                    return NotFound("No se encontraron datos para la casa de cambio especificada.");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

    }
}
