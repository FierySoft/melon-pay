namespace MelonPay.Models
{
    public class InvoiceCreate
    {
        public int FromWalletId { get; set; }
        public int ToWalletId { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
    }
}
