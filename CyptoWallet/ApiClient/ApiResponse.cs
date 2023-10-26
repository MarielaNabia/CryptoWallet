using CyptoWallet.ApiClient;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace CyptoWallet.ApiClient
{
    public class ApiResponse
    {
        public Status Status { get; set; }
        public Dictionary<string, CryptoData> Data { get; set; }
    }

    public class Status
    {
        public DateTime Timestamp { get; set; }
        public int ErrorCode { get; set; }
        public string ErrorMessage { get; set; }
        public int Elapsed { get; set; }
        public int CreditCount { get; set; }
        public string Notice { get; set; }
    }

    public class CryptoData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public decimal PriceUSD { get; set; }
        public string Logo { get; set; }
        
    }
}


