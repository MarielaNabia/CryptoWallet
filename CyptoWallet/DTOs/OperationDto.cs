namespace CyptoWallet.DTOs
{
    public class OperationDto
    {
        public DateTime Timestamp { get; set; }
        public decimal Amount { get; set; }
        public string SourceAccountName { get; set; }
        public string DestinationAccountName { get; set; }
        public string OperationType { get; set; }
        public string AccountType { get; set; }
    }
}
