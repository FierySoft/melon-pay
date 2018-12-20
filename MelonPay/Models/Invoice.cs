using System;

namespace MelonPay.Models
{
    public class Invoice
    {
        public long Id { get; set; }
        public decimal Amount { get; set; }
        public string Comment { get; set; }
        public DateTime CreatedAt { get; set; }

        public int FromWalletId { get; set; }
        public Wallet FromWallet { get; set; }

        public int ToWalletId { get; set; }
        public Wallet ToWallet { get; set; }

        public int StatusId { get; set; }
        public InvoiceStatus Status { get; set; }
    }
}
