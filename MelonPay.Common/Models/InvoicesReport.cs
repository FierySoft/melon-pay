using MelonPay.Common.Entities;

namespace MelonPay.Common.Models
{
    public class InvoicesReport
    {
        public int CardHolderId { get; set; }
        public CardHolder CardHolder { get; set; }
        public Invoice[] Sended { get; set; }
        public Invoice[] Received { get; set; }
    }
}
