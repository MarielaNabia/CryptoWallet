using CyptoWallet.DTOs;
using System.Globalization;
using System.Text.Json;

public class CriptoApiClient
{
    private readonly IConfiguration _configuration;
    private readonly HttpClient _httpClient;

    public CriptoApiClient(IConfiguration configuration)
    {
        _configuration = configuration;
        _httpClient = new HttpClient();
    }

    public async Task<CryptoDto> GetCryptocurrencyInfo(string cryptocurrencyId)
    {
        try
        {
            string baseUrl = _configuration["CoinMarketCapApi:BaseUrl"];
            string apiKey = _configuration["CoinMarketCapApi:ApiKey"];     

            if (string.IsNullOrEmpty(baseUrl) || string.IsNullOrEmpty(apiKey))
            {
                throw new InvalidOperationException("CoinMarketCap API configuration is missing.");
            }

            //URL para la solicitud a la API de CoinMarketCap
            string apiUrl = $"{baseUrl}/cryptocurrency/info?id={cryptocurrencyId}";

            // la clave de API
            _httpClient.DefaultRequestHeaders.Add("X-CMC_PRO_API_KEY", apiKey);

            HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

            if (response.IsSuccessStatusCode)
            {
                string cryptocurrencyData = await response.Content.ReadAsStringAsync();//para leer como cadena de caracteres

                if(cryptocurrencyId == "1") 
                {

                    var cryptoDto = ProcessCryptocurrencyData(cryptocurrencyData);

                    return cryptoDto;
                }
                else
                {
                    var cryptoDto = ProcessCryptocurrencyDataNoBTC(cryptocurrencyData, cryptocurrencyId);

                    return cryptoDto;
                }              
            }
            else
            {
                throw new Exception($"Error al llamar a la API: {response.StatusCode} - {response.ReasonPhrase}");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al llamar a la API: {ex.Message}");
        }
    }

    private CryptoDto ProcessCryptocurrencyData(string cryptocurrencyData)
    {
        using (JsonDocument document = JsonDocument.Parse(cryptocurrencyData))
        {
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement)//si el bloque tiene la prop data
               && dataElement.TryGetProperty("1", out var cryptoDataElement)) // y si dentro de ella con clave 1 q es BTC
            {
                var cryptoDto = new CryptoDto
                {
                    Name = cryptoDataElement.GetProperty("name").GetString(),
                    Symbol = cryptoDataElement.GetProperty("symbol").GetString(),
                    Logo = cryptoDataElement.GetProperty("logo").GetString(),                 
                };
                
                var description = cryptoDataElement.GetProperty("description").GetString();
                var priceStartIndex = description.IndexOf("The last known price of");//busco indices en la desc para ver donde comienza
                var priceEndIndex = description.IndexOf("USD", priceStartIndex);//y donde termina

                if (priceStartIndex >= 0 && priceEndIndex > priceStartIndex)
                {
                    var priceString = description.Substring(priceStartIndex, priceEndIndex - priceStartIndex);//me quedo con la descripcion
                    var priceParts = priceString.Split("is", StringSplitOptions.RemoveEmptyEntries);//divido el texto usando is

                    if (priceParts.Length >= 1)
                    {
                        string priceValue = priceParts[1].Trim();
                        
                        // Reemplaza las comas con espacios vacíos y los puntos con comas
                        priceValue = priceValue.Replace(",", "").Replace(".", ",");

                        // Divide el número en partes enteras y decimales
                        string[] parts = priceValue.Split(',');

                        if (parts.Length == 2)
                        {
                            // Formatea las partes enteras y decimales con comas y puntos
                            string formattedPrice = string.Format("{0:N0},{1}", Convert.ToDecimal(parts[0]), parts[1]);
                                                    
                            var cultureInfo = new CultureInfo("es-ES"); 

                        if (decimal.TryParse(formattedPrice, NumberStyles.Any, cultureInfo, out var price))
                        {
                            cryptoDto.PriceUSD = Math.Round(price, 2);

                        }
                    }
                }

                return cryptoDto;
            }
        }
        throw new Exception("No se encontraron datos para la criptomoneda.");
    }
    }

    private CryptoDto ProcessCryptocurrencyDataNoBTC(string cryptocurrencyData, string cryptocurrencyId)
    {
        using (JsonDocument document = JsonDocument.Parse(cryptocurrencyData))
        {
            var root = document.RootElement;

            if (root.TryGetProperty("data", out var dataElement) //si el bloque tiene la prop data
              && dataElement.TryGetProperty(cryptocurrencyId, out var cryptoDataElement)) 
            {
                var cryptoDto = new CryptoDto
                {
                    Name = cryptoDataElement.GetProperty("name").GetString(),
                    Symbol = cryptoDataElement.GetProperty("symbol").GetString(),
                    Logo = cryptoDataElement.GetProperty("logo").GetString(),
                   
                };

                
                var description = cryptoDataElement.GetProperty("description").GetString();
                var priceStartIndex = description.IndexOf("The last known price of");
                var priceEndIndex = description.IndexOf("USD", priceStartIndex);

                if (priceStartIndex >= 0 && priceEndIndex > priceStartIndex)
                {
                    var priceString = description.Substring(priceStartIndex, priceEndIndex - priceStartIndex);
                    var priceParts = priceString.Split("is", StringSplitOptions.RemoveEmptyEntries);

                    if (priceParts.Length >= 1)
                    {
                        string priceValue = priceParts[1].Trim();

                        // Reemplaza las comas con espacios vacíos y los puntos con comas
                        priceValue = priceValue.Replace(",", "").Replace(".", ",");

                        // Divide el número en partes enteras y decimales
                        string[] parts = priceValue.Split(',');

                        if (parts.Length == 2)
                        {
                            // Formatea las partes enteras y decimales con comas y puntos
                            string formattedPrice = string.Format("{0:N0},{1}", Convert.ToDecimal(parts[0]), parts[1]);
                          
                            var cultureInfo = new CultureInfo("es-ES");
                           
                            if (decimal.TryParse(formattedPrice, NumberStyles.Any, cultureInfo, out var price))
                            {                            
                                cryptoDto.PriceUSD = Math.Round(price, 2);
                            }
                        }
                    }

                    return cryptoDto;
                }
            }
            throw new Exception("No se encontraron datos para la criptomoneda.");
        }
    }
}
