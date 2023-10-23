namespace CyptoWallet.DTOs
{
    public class TransferRequestDto
    {
        public string SourceAliasOrCBU { get; set; }
        public string DestinationAliasOrCBU { get; set; }
        public decimal Amount { get; set; }
        //public int SourceAccountId { get; set; }
        //public int DestinationAccountId { get; set; }
        //public decimal Amount { get; set; }
    }
    public class TransferResponseDto
    {
        public string Message { get; set; }
        public decimal NewSourceAccountBalance { get; set; }
        public decimal NewDestinationAccountBalance { get; set; }
    }
}
