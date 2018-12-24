using MelonPay.Persisted.DbContexts;
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

            return services;
        }
    }
}
