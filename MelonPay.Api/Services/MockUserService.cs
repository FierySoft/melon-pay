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


        public async Task<UserAccount> GetSignedInAsync()
        {
            var account = await _accounts.GetByIdAsync(1);

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
