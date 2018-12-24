using System;
using System.Linq;
using System.Threading.Tasks;
using MelonPay.Common.Entities;
using MelonPay.Common.Abstractions;
using MelonPay.InMemory.DbContexts;

namespace MelonPay.InMemory.DataSources
{
    public class InMemoryWalletRepository : IWalletRepository
    {
        private readonly InMemoryDbContext _db;

        public InMemoryWalletRepository(InMemoryDbContext context)
        {
            _db = context;
        }


        public Task<WalletPrivate[]> GetAllAsync()
        {
            return Task.FromResult(_db.Wallets
                .Where(x => x.IsDeleted == false)
                .ToArray());
        }

        public Task<WalletPrivate> GetByIdAsync(int id)
        {
            var model = _db.Wallets.First(x => x.Id == id);
            model.Currency = _db.Currencies.First(x => x.Id == model.CurrencyId);
            model.CardHolder = _db.CardHolders.FirstOrDefault(p => p.Id == model.CardHolderId);

            return Task.FromResult(model);
        }

        public Task<WalletPrivate> CreateAsync(WalletPrivate model)
        {
            model.Id = _db.Wallets.Count() > 0 ? _db.Accounts.Last().Id + 1 : 1;
            model.IsDeleted = false;
            _db.Wallets.Add(model);
            return Task.FromResult(model);
        }

        public Task<WalletPrivate> UpdateAsync(WalletPrivate model)
        {
            var update = _db.Wallets.First(x => x.Id == model.Id);

            return Task.FromResult(update);
        }

        public Task DeleteAsync(int id)
        {
            var model = _db.Wallets.First(x => x.Id == id);
            model.IsDeleted = true;

            return Task.FromResult(0);
        }

        public Task DeleteAsync(WalletPrivate model)
        {
            model.IsDeleted = true;

            return Task.FromResult(0);
        }

        public Task<Wallet[]> GetByCardHolderIdAsync(int cardHolderId)
        {
            return Task.FromResult(_db.Wallets
                .Where(x => x.CardHolderId == cardHolderId && x.IsDeleted == false)
                .Select(x => x as Wallet)
                .ToArray());
        }

        public Task<Wallet[]> GetByAccountIdAsync(int accountId)
        {
            var account = _db.Accounts.FirstOrDefault(x => x.Id == accountId);
            return Task.FromResult(_db.Wallets
                .Where(x => x.CardHolderId == account.CardHolderId && x.IsDeleted == false)
                .Select(x => x as Wallet)
                .ToArray());
        }

        public async Task<Wallet> AddAmountAsync(int walletId, decimal amount)
        {
            var model = _db.Wallets.First(x => x.Id == walletId);
            model.Balance += amount;
            return await UpdateAsync(model);
        }

        public async Task<Wallet> ReduceAmountAsync(int walletId, decimal amount)
        {
            var model = _db.Wallets.First(x => x.Id == walletId);
            if (model.Balance < amount)
            {
                throw new ArgumentException("На кошельке недостаточно средств для списания");
            }

            model.Balance -= amount;
            return await UpdateAsync(model);
        }
    }
}
