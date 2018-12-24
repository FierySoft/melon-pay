using MelonPay.Common.Entities;
using System.Threading.Tasks;

namespace MelonPay.Common.Abstractions
{
    public interface IWalletRepository : ICatalogueRepository<WalletPrivate>
    {
        Task<Wallet[]> GetByCardHolderIdAsync(int cardHolderId);
        Task<Wallet[]> GetByAccountIdAsync(int accountId);
        Task<Wallet> AddAmountAsync(int walletId, decimal amount);
        Task<Wallet> ReduceAmountAsync(int walletId, decimal amount);
    }
}
