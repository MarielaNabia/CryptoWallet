namespace CyptoWallet.Entities
{
    public class AccountType
    {
        public int AccountTypeId { get; set; }
        public string Name { get; set; }
        

        // Relación con las cuentas
        public ICollection<Account> Accounts { get; set; }
    }
    public static class AccountTypes
    {
        public const string Pesos = "Pesos";
        public const string Dolares = "Dolares";
        public const string BTC = "BTC";
        // Agrega más tipos de cuentas según tus necesidades
    }
}
