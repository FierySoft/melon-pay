using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MelonPay.Common.Models;
using MelonPay.Common.Entities;
using MelonPay.Common.Abstractions;
using MelonPay.Persisted.DbContexts;

namespace MelonPay.PersistentDb.DataSources
{
    class PersistedInvoiceRepository : IInvoiceRepository
    {
        private readonly MelonPayDbContext _db;
        private readonly ICardHolderRepository _cardHolders;
        private readonly IWalletRepository _wallets;

        public PersistedInvoiceRepository(MelonPayDbContext context, ICardHolderRepository cardHolders, IWalletRepository wallets)
        {
            _db = context;
            _cardHolders = cardHolders;
            _wallets = wallets;
        }


        public async Task<InvoicesReport> GetByCardHolderIdAsync(int cardHolderId)
        {
            var cardHolder = await _cardHolders.GetByIdAsync(cardHolderId);
            cardHolder.Wallets = cardHolder.Wallets.Select(x => { x.CardHolder = null; return x; });

            var query = _db.Invoices
                .AsNoTracking()
                .Include(x => x.FromWallet)
                .ThenInclude(x => x.Currency)
                .Include(x => x.ToWallet)
                .ThenInclude(x => x.Currency)
                .Include(X => X.Status);

            var sended = await query.Where(x => cardHolder.Wallets
                .Select(w => w.Id)
                .Contains(x.FromWalletId.Value)
                ).ToArrayAsync();

            var received = await query.Where(x => cardHolder.Wallets
                .Select(w => w.Id)
                .Contains(x.ToWalletId.Value)
                ).ToArrayAsync();

            return new InvoicesReport
            {
                CardHolderId = cardHolderId,
                CardHolder = cardHolder,
                Sended = sended,
                Received = received
            };
        }

        public async Task<Invoice> ChangeStatusAsync(long invoiceId, int cardHolderId, InvoiceStatus status)
        {
            var invoice = _db.Invoices.FirstOrDefault(x => x.Id == invoiceId);
            invoice.Status = status;
            // TODO: add permissions check

            _db.Invoices.Update(invoice);
            await _db.SaveChangesAsync();

            return invoice;
        }

        public async Task<Invoice> CreateAsync(int fromWalletId, int toWalletId, decimal amount, string comment = null)
        {
            var status = _db.InvoiceStatuses.First(x => x.Code == "New");

            var invoice = new Invoice
            {
                Id = _db.Wallets.Count() > 0 ? _db.Accounts.Last().Id + 1 : 1,
                FromWalletId = fromWalletId,
                ToWalletId = toWalletId,
                Amount = amount,
                Comment = comment,
                StatusId = status.Id,
                Status = status,
                CreatedAt = DateTime.UtcNow
            };

            _db.Invoices.Add(invoice);
            await _db.SaveChangesAsync();

            return invoice;
        }

        public async Task<Invoice> PayAsync(int id)
        {
            var invoice = _db.Invoices.First(x => x.Id == id);

            // ACID transaction
            var result = await TransactionAsync(invoice.FromWalletId.Value, invoice.ToWalletId.Value, invoice.Amount);

            if (result.Success)
            {
                var status = _db.InvoiceStatuses.First(x => x.Code == "Payed");
                invoice.StatusId = status.Id;
                invoice.Status = status;
                invoice.Reason = "Ok";

                _db.Invoices.Add(invoice);
                await _db.SaveChangesAsync();

                return invoice;
            }
            else
            {
                var status = _db.InvoiceStatuses.First(x => x.Code == "Error");
                invoice.StatusId = status.Id;
                invoice.Status = status;
                invoice.Reason = result.ErrorReason;

                _db.Invoices.Add(invoice);
                await _db.SaveChangesAsync();

                return invoice;
            }
        }

        public async Task<Invoice> DeclineAsync(int id)
        {
            var invoice = _db.Invoices.First(x => x.Id == id);
            var status = _db.InvoiceStatuses.First(x => x.Code == "Declined");
            invoice.StatusId = status.Id;
            invoice.Status = status;

            _db.Invoices.Add(invoice);
            await _db.SaveChangesAsync();

            return invoice;
        }


        private async Task<TransactionResult> TransactionAsync(int fromWalletId, int toWalletId, decimal amount)
        {
            try
            {
                await _wallets.ReduceAmountAsync(fromWalletId, amount);
            }
            catch (Exception ex)
            {
                return TransactionResult.FromException(ex);
            }

            await _wallets.AddAmountAsync(toWalletId, amount);
            return TransactionResult.FromSuccess();
        }
    }
}
