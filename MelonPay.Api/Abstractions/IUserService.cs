using MelonPay.Api.Models;
using System.Threading.Tasks;

namespace MelonPay.Api.Abstractions
{
    public interface IUserService
    {
        Task<UserAccount[]> GetAllAsync();
        Task<UserAccount> GetSignedInAsync(int id);
    }
}
