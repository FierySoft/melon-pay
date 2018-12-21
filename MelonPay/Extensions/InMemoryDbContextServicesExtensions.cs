using MelonPay.Abstractions;
using MelonPay.DataSources;
using MelonPay.DbContexts;
using MelonPay.Entities;

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
