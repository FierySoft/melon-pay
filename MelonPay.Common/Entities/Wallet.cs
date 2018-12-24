namespace MelonPay.Common.Entities
{
    public class Wallet
    {
        public int Id { get; set; }
        public int CurrencyId { get; set; }
        public Currency Currency { get; set; }
        public int CardHolderId { get; set; }
        public CardHolder CardHolder { get; set; }
    }

    public class WalletPrivate : Wallet
    {
        public decimal Balance { get; set; }
        public bool IsDeleted { get; set; }
    }
}
