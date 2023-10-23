namespace CyptoWallet.DTOs
{
    public class CryptoDto
    {
        public string Name { get; set; } // Nombre de la criptomoneda
        public decimal PriceUSD { get; set; } // Precio de compra en USD
        public string Symbol { get; set; } // Símbolo de la criptomoneda (por ejemplo, BTC)
        public string Logo { get; set; } // URL de la imagen del símbolo (logo)
                                         // Otros valores relevantes según tus necesidades
    }
}
