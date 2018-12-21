using MelonPay.Entities;

namespace MelonPay.Models
{
    public class InvoicesReport
    {
        public int CardHolderId { get; set; }
        public CardHolder CardHolder { get; set; }
        public Invoice[] Sended { get; set; }
        public Invoice[] Received { get; set; }
    }
}
