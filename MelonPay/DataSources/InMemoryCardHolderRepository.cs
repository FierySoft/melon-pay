using System.Linq;
using System.Threading.Tasks;
using MelonPay.Abstractions;
using MelonPay.DbContexts;
using MelonPay.Entities;

namespace MelonPay.DataSources
{
    public class InMemoryCardHolderRepository : ICardHolderRepository
    {
        private readonly InMemoryDbContext _db;

        public InMemoryCardHolderRepository(InMemoryDbContext context)
        {
            _db = context;
        }


        public Task<CardHolder[]> GetAllAsync()
        {
            return Task.FromResult(_db.CardHolders.ToArray());
        }

        public Task<CardHolder> GetByIdAsync(int id)
        {
            var model = _db.CardHolders.First(x => x.Id == id);
            model.Wallets = _db.Wallets.Where(x => x.CardHolderId == id && x.IsDeleted == false);

            return Task.FromResult(model);
        }

        public Task<CardHolder> CreateAsync(CardHolder model)
        {
            model.Id = _db.CardHolders.Count() > 0 ? _db.CardHolders.Last().Id + 1 : 1;

            _db.CardHolders.Add(model);

            return Task.FromResult(model);
        }

        public Task<CardHolder> UpdateAsync(CardHolder model)
        {
            var update = _db.CardHolders.First(x => x.Id == model.Id);

            update.FirstName = model.FirstName;
            update.MiddleName = model.MiddleName;
            update.LastName = model.LastName;
            update.Gender = model.Gender;

            return Task.FromResult(update);
        }

        public Task DeleteAsync(int id)
        {
            var model = _db.CardHolders.First(x => x.Id == id);
            model.IsDeleted = true;
            return Task.FromResult(0);
        }

        public Task DeleteAsync(CardHolder model)
        {
            model.IsDeleted = true;
            return Task.FromResult(0);
        }
    }
}
