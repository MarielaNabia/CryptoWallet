using System;
using System.Dynamic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using CyptoWallet.DTOs;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace CyptoWallet.ApiClient
{
    public class DolarApiClient
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public DolarApiClient(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<List<DolarInfoDto>> GetAllDollarQuotes(string tipoCasa)
        {
            try
            {
                string baseUrl = _configuration["DolarApi:BaseUrl"];

                if (string.IsNullOrEmpty(baseUrl))
                {
                    throw new InvalidOperationException("Dolar API configuration is missing.");
                }

                string apiUrl = $"{baseUrl}/v1/dolares";

                HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

                if (response.IsSuccessStatusCode)
                {
                    string dolarData = await response.Content.ReadAsStringAsync();

                    // Deserializa la respuesta JSON 
                    var dolarQuotes = JsonConvert.DeserializeObject<List<dynamic>>(dolarData);
                    
                    var bolsaQuote = dolarQuotes.FirstOrDefault(item => item.casa == tipoCasa);

                    if (bolsaQuote != null)
                    {
                        var dolarInfoDto = new DolarInfoDto
                        {
                            Casa = bolsaQuote.casa,
                            Nombre = bolsaQuote.nombre,
                            Compra = bolsaQuote.compra,
                            Venta = bolsaQuote.venta,
                            FechaActualizacion = bolsaQuote.fechaActualizacion
                        };


                        var result = new List<DolarInfoDto> { dolarInfoDto };

                        return result;
                    }
                }

                // En caso de que no se encuentre la cotización "bolsa" o haya un error, devuelve una lista vacía 
                return new List<DolarInfoDto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al llamar a la API de Dolar: {ex.Message}");
            }
        }


        public async Task<DolarInfoDto> GetDollarQuote(string casa)
        {         
            var allDollarQuotes = await GetAllDollarQuotes(casa);

            // Busca la cotización específica por la casa ("bolsa" en este caso)
            var bolsaDollarQuote = allDollarQuotes.FirstOrDefault(quote => quote.Casa == casa);

            return bolsaDollarQuote;
           
        }
    }

}
