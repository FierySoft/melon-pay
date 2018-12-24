using System;
using System.Linq;
using System.Threading.Tasks;
using MelonPay.Common.Entities;
using MelonPay.Common.Abstractions;
using MelonPay.InMemory.DbContexts;

namespace MelonPay.InMemory.DataSources
{
    public class InMemoryAccountsRepository : ICatalogueRepository<Account>
    {
        private readonly InMemoryDbContext _db;

        public InMemoryAccountsRepository(InMemoryDbContext context)
        {
            _db = context;
        }


        public Task<Account[]> GetAllAsync()
        {
            return Task.FromResult(_db.Accounts
                .Where(x => x.IsDeleted == false)
                .ToArray());
        }

        public Task<Account> GetByIdAsync(int id)
        {
            var model = _db.Accounts.First(x => x.Id == id);
            model.CardHolder = _db.CardHolders.FirstOrDefault(p => p.Id == model.CardHolderId);
            model.CardHolder.Wallets = _db.Wallets.Where(x => x.CardHolderId == model.CardHolderId);

            return Task.FromResult(model);
        }

        public Task<Account> CreateAsync(Account model)
        {
            model.CardHolder.Id = _db.CardHolders.Count() > 0 ? _db.CardHolders.Last().Id + 1 : 1;
            model.CardHolder.IsDeleted = false;
            _db.CardHolders.Add(model.CardHolder);

            model.Id = _db.Accounts.Count() > 0 ? _db.Accounts.Last().Id + 1 : 1;
            model.CardHolderId = model.CardHolder.Id;
            model.CreatedAt = DateTime.UtcNow;
            model.IsDeleted = false;
            _db.Accounts.Add(model);

            var wallet = new WalletPrivate
            {
                Id = _db.Wallets.Count() > 0 ? _db.Wallets.Last().Id + 1 : 1,
                CardHolderId = model.CardHolderId,
                CardHolder = model.CardHolder,
                CurrencyId = 1,
                Currency = _db.Currencies.First(x => x.Id == 1),
                Balance = 0m,
                IsDeleted = false
            };
            _db.Wallets.Add(wallet);
            model.CardHolder.Wallets = new Wallet[1] { wallet };

            return Task.FromResult(model);
        }

        public Task<Account> UpdateAsync(Account model)
        {
            var update = _db.Accounts.First(x => x.Id == model.Id);

            update.UserName = model.UserName;
            update.Password = model.Password;

            return Task.FromResult(update);
        }

        public Task DeleteAsync(int id)
        {
            var model = _db.Accounts.First(x => x.Id == id);

            var wallets = _db.Wallets.Where(x => x.CardHolderId == model.CardHolderId);
            wallets.Select(x => x.IsDeleted = true);

            var cardHolder = _db.CardHolders.FirstOrDefault(p => p.Id == model.CardHolderId);
            cardHolder.IsDeleted = true;

            model.IsDeleted = true;

            return Task.FromResult(0);
        }

        public Task DeleteAsync(Account model)
        {
            var wallets = _db.Wallets.Where(x => x.CardHolderId == model.CardHolderId);
            wallets.Select(x => x.IsDeleted == true);

            var cardHolder = _db.CardHolders.FirstOrDefault(p => p.Id == model.CardHolderId);
            cardHolder.IsDeleted = true;

            model.IsDeleted = true;

            return Task.FromResult(0);
        }
    }
}
