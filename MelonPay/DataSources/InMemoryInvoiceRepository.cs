using System;
using System.Linq;
using System.Threading.Tasks;
using MelonPay.Abstractions;
using MelonPay.DbContexts;
using MelonPay.Entities;
using MelonPay.Models;

namespace MelonPay.DataSources
{
    public class InMemoryInvoiceRepository : IInvoiceRepository
    {
        private readonly InMemoryDbContext _db;
        private readonly ICardHolderRepository _cardHolders;
        private readonly IWalletRepository _wallets;

        public InMemoryInvoiceRepository(InMemoryDbContext context, ICardHolderRepository cardHolders, IWalletRepository wallets)
        {
            _db = context;
            _cardHolders = cardHolders;
            _wallets = wallets;
        }


        public async Task<InvoicesReport> GetByCardHolderIdAsync(int cardHolderId)
        {
            var cardHolder = await _cardHolders.GetByIdAsync(cardHolderId);

            var sended = _db.Invoices.Where(x => cardHolder.Wallets.Select(w => w.Id).Contains(x.FromWalletId)).ToArray();
                /*await Task.WhenAll(_db.Invoices
                .Where(x => cardHolder.Wallets.Select(w => w.Id).Contains(x.FromWalletId))
                .Select(async x =>
                {
                    x.FromWallet = await _wallets.GetByIdAsync(x.FromWalletId);
                    x.ToWallet = await _wallets.GetByIdAsync(x.ToWalletId);
                    return x;
                }));*/

            var received = _db.Invoices.Where(x => cardHolder.Wallets.Select(w => w.Id).Contains(x.ToWalletId)).ToArray();
                /*await Task.WhenAll(_db.Invoices
                .Where(x => cardHolder.Wallets.Select(w => w.Id).Contains(x.ToWalletId))
                .Select(async x => {
                    x.FromWallet = await _wallets.GetByIdAsync(x.FromWalletId);
                    x.ToWallet = await _wallets.GetByIdAsync(x.ToWalletId);
                    return x;
                }));*/

            return new InvoicesReport
            {
                CardHolderId = cardHolderId,
                CardHolder = cardHolder,
                Sended = sended,
                Received = received
            };
        }

        public Task<Invoice> ChangeStatusAsync(long invoiceId, int cardHolderId, InvoiceStatus status)
        {
            var invoice = _db.Invoices.FirstOrDefault(x => x.Id == invoiceId);
            invoice.Status = status;
            // TODO: add permissions check
            return Task.FromResult(invoice);
        }
    }
}
