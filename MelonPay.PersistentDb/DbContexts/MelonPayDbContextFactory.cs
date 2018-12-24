using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Options;

namespace MelonPay.Persisted.DbContexts
{
    public class MelonPayDbContextFactory : IDesignTimeDbContextFactory<MelonPayDbContext>
    {
        private readonly MelonPayDbContextOptions _options;

        public MelonPayDbContextFactory(IOptions<MelonPayDbContextOptions> optionAccessor)
        {
            _options = optionAccessor.Value;
        }

        public MelonPayDbContextFactory()
        {

        }


        public MelonPayDbContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<MelonPayDbContext>();
            //optionsBuilder.UseSqlServer(_options.ConnectionString);
            optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=MelonPayDb;Trusted_Connection=True;", b => b.MigrationsAssembly("MelonPay.PersistentDb"));

            return new MelonPayDbContext(optionsBuilder.Options);
        }
    }
}
