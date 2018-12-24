using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MelonPay.Common.Entities;
using MelonPay.Common.Abstractions;
using MelonPay.Persisted.DbContexts;

namespace MelonPay.PersistentDb.DataSources
{
    class PersistedWalletRepository : IWalletRepository
    {
        private readonly MelonPayDbContext _db;

        public PersistedWalletRepository(MelonPayDbContext context)
        {
            _db = context;
        }


        public Task<WalletPrivate[]> GetAllAsync()
        {
            return _db.Wallets
                .AsNoTracking()
                .Include(x => x.Currency)
                .Include(x => x.CardHolder)
                .Where(x => x.IsDeleted == false)
                .ToArrayAsync();
        }

        public Task<WalletPrivate> GetByIdAsync(int id)
        {
            return _db.Wallets
                .AsNoTracking()
                .Include(x => x.Currency)
                .Include(x => x.CardHolder)
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalletPrivate> CreateAsync(WalletPrivate model)
        {
            model.IsDeleted = false;
            _db.Wallets.Add(model);
            await _db.SaveChangesAsync();

            return model;
        }

        public async Task<WalletPrivate> UpdateAsync(WalletPrivate model)
        {
            var update = await _db.Wallets.FirstOrDefaultAsync(x => x.Id == model.Id);

            update.Balance = model.Balance;
            _db.Wallets.Update(model);
            await _db.SaveChangesAsync();

            return update;
        }

        public async Task DeleteAsync(int id)
        {
            var model = await _db.Wallets.FirstOrDefaultAsync(x => x.Id == id);
            await DeleteAsync(model);
        }

        public Task DeleteAsync(WalletPrivate model)
        {
            model.IsDeleted = true;

            _db.Update(model);
            return _db.SaveChangesAsync();
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
            var account = _db.Accounts.First(x => x.Id == accountId);

            return _db.Wallets
                .Include(x => x.Currency)
                .Include(x => x.CardHolder)
                .Where(x => x.CardHolderId == account.CardHolderId && x.IsDeleted == false)
                .Select(x => x as Wallet)
                .ToArrayAsync();
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
