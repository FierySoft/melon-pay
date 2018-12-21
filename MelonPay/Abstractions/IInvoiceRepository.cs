using MelonPay.Models;
using MelonPay.Entities;
using System.Threading.Tasks;

namespace MelonPay.Abstractions
{
    public interface IInvoiceRepository
    {
        Task<InvoicesReport> GetByCardHolderIdAsync(int cardHolderId);
        Task<Invoice> ChangeStatusAsync(long invoiceId, int cardHolderId, InvoiceStatus status);
    }
}
