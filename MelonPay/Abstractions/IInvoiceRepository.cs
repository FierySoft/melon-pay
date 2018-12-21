using MelonPay.Models;
using MelonPay.Entities;
using System.Threading.Tasks;

namespace MelonPay.Abstractions
{
    public interface IInvoiceRepository
    {
        Task<InvoicesReport> GetByCardHolderIdAsync(int cardHolderId);
        Task<Invoice> ChangeStatusAsync(long invoiceId, int cardHolderId, InvoiceStatus status);
        Task<Invoice> CreateAsync(int fromWalletId, int toWalletId, decimal amount, string comment = null);
        Task<Invoice> PayAsync(int id);
        Task<Invoice> DeclineAsync(int id);
    }
}
