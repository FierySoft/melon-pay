using MelonPay.Common.Abstractions;
using MelonPay.Common.Entities;
using MelonPay.Persisted.DbContexts;
using MelonPay.PersistentDb.DataSources;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class PersistedDbContextServicesExtensions
    {
        public static IServiceCollection AddPersistedServices(this IServiceCollection services, IConfiguration configuration)
        {
            //services.Configure<MelonPayDbContextOptions>(configuration.GetSection("MelonPayDbContext"));
            services.AddDbContext<MelonPayDbContext>(options => options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddTransient<ICatalogueRepository<Account>, PersistedAccountRepository>();
            services.AddTransient<ICardHolderRepository, PersistedCardHolderRepository>();
            services.AddTransient<IWalletRepository, PersistedWalletRepository>();
            services.AddTransient<IInvoiceRepository, PersistedInvoiceRepository>();

            return services;
        }
    }
}
