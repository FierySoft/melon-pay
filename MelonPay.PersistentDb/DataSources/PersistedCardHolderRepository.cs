using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MelonPay.Common.Entities;
using MelonPay.Common.Abstractions;
using MelonPay.Persisted.DbContexts;

namespace MelonPay.PersistentDb.DataSources
{
    class PersistedCardHolderRepository : ICardHolderRepository
    {
        private readonly MelonPayDbContext _db;

        public PersistedCardHolderRepository(MelonPayDbContext context)
        {
            _db = context;
        }


        public Task<CardHolder[]> GetAllAsync()
        {
            return _db.CardHolders
                .AsNoTracking()
                .Where(x => x.IsDeleted == false)
                .ToArrayAsync();
        }

        public async Task<CardHolder> GetByIdAsync(int id)
        {
            var value = await _db.CardHolders
                .AsNoTracking()
                .Include(x => x.Wallets)
                .ThenInclude(x => x.Currency)
                .FirstOrDefaultAsync(x => x.Id == id);

            Array.ForEach(value.Wallets.ToArray(), w => w.CardHolder = null);

            return value;
        }

        public async Task<CardHolder> CreateAsync(CardHolder model)
        {
            model.IsDeleted = false;
            _db.CardHolders.Add(model);
            await _db.SaveChangesAsync();

            return model;
        }

        public async Task<CardHolder> UpdateAsync(CardHolder model)
        {
            var update = await _db.CardHolders.FirstAsync(x => x.Id == model.Id);

            update.FirstName = model.FirstName;
            update.MiddleName = model.MiddleName;
            update.LastName = model.LastName;
            update.Gender = model.Gender;

            _db.CardHolders.Update(update);
            await _db.SaveChangesAsync();

            return update;
        }

        public async Task DeleteAsync(int id)
        {
            var model = await _db.CardHolders.FirstAsync(x => x.Id == id);
            await DeleteAsync(model);

        }

        public Task DeleteAsync(CardHolder model)
        {
            model.IsDeleted = true;

            _db.Update(model);
            return _db.SaveChangesAsync();
        }
    }
}
