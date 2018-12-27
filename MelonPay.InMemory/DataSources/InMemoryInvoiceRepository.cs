using System;
using System.Linq;
using System.Threading.Tasks;
using MelonPay.Common.Models;
using MelonPay.Common.Entities;
using MelonPay.Common.Abstractions;
using MelonPay.InMemory.DbContexts;

namespace MelonPay.InMemory.DataSources
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

            var sended = _db.Invoices.Where(x => cardHolder.Wallets.Select(w => w.Id).Contains(x.FromWalletId.Value)).ToArray();
                /*await Task.WhenAll(_db.Invoices
                .Where(x => cardHolder.Wallets.Select(w => w.Id).Contains(x.FromWalletId))
                .Select(async x =>
                {
                    x.FromWallet = await _wallets.GetByIdAsync(x.FromWalletId);
                    x.ToWallet = await _wallets.GetByIdAsync(x.ToWalletId);
                    return x;
                }));*/

            var received = _db.Invoices.Where(x => cardHolder.Wallets.Select(w => w.Id).Contains(x.ToWalletId.Value)).ToArray();
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

        public async Task<InvoicesReport> GetByWalletIdAsync(int cardHolderId, int walletId)
        {
            var cardHolder = await _cardHolders.GetByIdAsync(cardHolderId);
            var wallet = await _wallets.GetByIdAsync(walletId);

            if (!cardHolder.Wallets.Select(x => x.Id).Contains(walletId))
            {
                throw new ArgumentException($"Wallet 3{walletId} does not belong to CardHolder 3{cardHolderId}");
            }

            var sended = _db.Invoices.Where(x => x.FromWalletId == walletId).ToArray();
            var received = _db.Invoices.Where(x => x.ToWalletId == walletId).ToArray();

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

        public Task<Invoice> CreateAsync(int fromWalletId, int toWalletId, decimal amount, string comment = null)
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
            return Task.FromResult(invoice);
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

                return invoice;
            }
            else
            {
                var status = _db.InvoiceStatuses.First(x => x.Code == "Error");
                invoice.StatusId = status.Id;
                invoice.Status = status;
                invoice.Reason = result.ErrorReason;

                return invoice;
            }
        }

        public Task<Invoice> DeclineAsync(int id)
        {
            var invoice = _db.Invoices.First(x => x.Id == id);
            var status = _db.InvoiceStatuses.First(x => x.Code == "Declined");
            invoice.StatusId = status.Id;
            invoice.Status = status;
            return Task.FromResult(invoice);
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
