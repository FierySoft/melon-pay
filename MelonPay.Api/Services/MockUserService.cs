using System.Linq;
using System.Threading.Tasks;
using MelonPay.Common.Abstractions;
using MelonPay.Api.Abstractions;
using MelonPay.Api.Models;

namespace MelonPay.Api.Services
{
    public class MockUserService : IUserService
    {
        private readonly IAccountRepository _accounts;

        public MockUserService(IAccountRepository accounts)
        {
            _accounts = accounts;
        }

        public async Task<UserAccount[]> GetAllAsync()
        {
            return (await _accounts.GetAllAsync()).Select(account => new UserAccount
            {
                Id = account.Id,
                UserName = account.UserName,
                CardHolderId = account.CardHolderId,
                CardHolder = account.CardHolder
            })
            .ToArray();
        }

        public async Task<UserAccount> GetSignedInAsync(int id)
        {
            var account = await _accounts.GetByIdAsync(id);

            return new UserAccount
            {
                Id = account.Id,
                UserName = account.UserName,
                CardHolderId = account.CardHolderId,
                CardHolder = account.CardHolder
            };
        }
    }
}
