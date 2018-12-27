using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MelonPay.Common.Entities;
using MelonPay.Common.Abstractions;
using MelonPay.Persisted.DbContexts;

namespace MelonPay.PersistentDb.DataSources
{
    class PersistedAccountRepository : IAccountRepository
    {
        private readonly MelonPayDbContext _db;

        public PersistedAccountRepository(MelonPayDbContext context)
        {
            _db = context;
        }


        public Task<Account[]> GetAllAsync()
        {
            return _db.Accounts
                .AsNoTracking()
                .Where(x => x.IsDeleted == false)
                .ToArrayAsync();
        }

        public async Task<Account> GetByIdAsync(int id)
        {
            var value = await _db.Accounts
                .AsNoTracking()
                .Include(x => x.CardHolder)
                .ThenInclude(x => x.Wallets)
                .ThenInclude(x => x.Currency)
                .FirstOrDefaultAsync(x => x.Id == id);

            Array.ForEach(value.CardHolder.Wallets.ToArray(), w => w.CardHolder = null);

            return value;
        }

        public async Task<Account> CreateAsync(Account model)
        {
            model.CardHolder.IsDeleted = false;
            _db.CardHolders.Add(model.CardHolder);
            await _db.SaveChangesAsync();

            model.CardHolderId = model.CardHolder.Id;
            model.CreatedAt = DateTime.UtcNow;
            model.IsDeleted = false;
            _db.Accounts.Add(model);
            await _db.SaveChangesAsync();

            var wallet = new WalletPrivate
            {
                CardHolderId = model.CardHolderId,
                CardHolder = model.CardHolder,
                CurrencyId = 1,
                Currency = _db.Currencies.First(x => x.Id == 1),
                Balance = 0m,
                IsDeleted = false
            };
            _db.Wallets.Add(wallet);
            await _db.SaveChangesAsync();

            model.CardHolder.Wallets = new Wallet[1] { wallet };

            return model;
        }

        public async Task<Account> UpdateAsync(Account model)
        {
            var update = await _db.Accounts.FirstAsync(x => x.Id == model.Id);

            update.UserName = model.UserName;
            update.Password = model.Password;

            _db.Accounts.Update(update);
            await _db.SaveChangesAsync();

            return update;
        }

        public async Task DeleteAsync(int id)
        {
            var model = await _db.Accounts.FirstAsync(x => x.Id == id);
            await DeleteAsync(model);
        }

        public async Task DeleteAsync(Account model)
        {
            var wallets = await _db.Wallets.Where(x => x.CardHolderId == model.CardHolderId).ToArrayAsync();
            wallets.Select(x => x.IsDeleted = true);
            _db.Wallets.UpdateRange(wallets);

            var cardHolder = await _db.CardHolders.FirstOrDefaultAsync(p => p.Id == model.CardHolderId);
            cardHolder.IsDeleted = true;
            _db.CardHolders.Update(cardHolder);

            model.IsDeleted = true;
            _db.Accounts.Update(model);

            await _db.SaveChangesAsync();
        }
    }
}
