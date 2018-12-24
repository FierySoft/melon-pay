using MelonPay.Common.Entities;
using MelonPay.Common.Abstractions;
using MelonPay.InMemory.DbContexts;
using MelonPay.InMemory.DataSources;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class InMemoryDbContextServicesExtensions
    {
        public static IServiceCollection AddInMemoryServices(this IServiceCollection services)
        {
            services.AddSingleton<InMemoryDbContext>();
            services.AddTransient<ICatalogueRepository<Account>, InMemoryAccountsRepository>();
            services.AddTransient<ICardHolderRepository, InMemoryCardHolderRepository>();
            services.AddTransient<IWalletRepository, InMemoryWalletRepository>();
            services.AddTransient<IInvoiceRepository, InMemoryInvoiceRepository>();

            return services;
        }
    }
}
