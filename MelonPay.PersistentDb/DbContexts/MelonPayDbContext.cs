using MelonPay.Common.Entities;
using Microsoft.EntityFrameworkCore;

namespace MelonPay.Persisted.DbContexts
{
    public class MelonPayDbContext : DbContext
    {
        public MelonPayDbContext(DbContextOptions<MelonPayDbContext> options)
            : base(options)
        {

        }


        public DbSet<Currency> Currencies { get; set; }
        public DbSet<InvoiceStatus> InvoiceStatuses { get; set; }
        public DbSet<CardHolder> CardHolders { get; set; }
        public DbSet<Account> Accounts { get; set; }
        public DbSet<WalletPrivate> Wallets { get; set; }
        public DbSet<Invoice> Invoices { get; set; }


        protected override void OnModelCreating(ModelBuilder builder)
        {
            var currencyEntity = builder.Entity<Currency>();
            currencyEntity.HasKey(x => x.Id);
            currencyEntity.Property(x => x.Code).IsRequired().HasMaxLength(3);
            currencyEntity.Property(x => x.Name).IsRequired();

            var invoiceStatusEntity = builder.Entity<InvoiceStatus>();
            currencyEntity.HasKey(x => x.Id);
            invoiceStatusEntity.Property(x => x.Code).IsRequired();
            invoiceStatusEntity.Property(x => x.Name).IsRequired();

            var cardHolderEntity = builder.Entity<CardHolder>();
            cardHolderEntity.HasKey(X => X.Id);
            cardHolderEntity.Property(x => x.FirstName).IsRequired().HasMaxLength(25);
            cardHolderEntity.Property(x => x.MiddleName).IsRequired();
            cardHolderEntity.Property(x => x.LastName).IsRequired().HasMaxLength(25);
            cardHolderEntity.HasMany(x => x.Wallets).WithOne(x => x.CardHolder).HasForeignKey(x => x.CardHolderId);

            var accountEntity = builder.Entity<Account>();
            accountEntity.HasKey(X => X.Id);
            accountEntity.Property(x => x.UserName).IsRequired().HasMaxLength(50);
            accountEntity.Property(x => x.Password).IsRequired().HasMaxLength(50);
            accountEntity.HasOne(x => x.CardHolder);

            var walletEntity = builder.Entity<Wallet>();
            walletEntity.HasKey(X => X.Id);
            walletEntity.HasOne(x => x.Currency);
            walletEntity.HasOne(x => x.CardHolder);

            var invoiceEntity = builder.Entity<Invoice>();
            currencyEntity.HasKey(x => x.Id);
            invoiceEntity.HasOne(x => x.FromWallet);
            invoiceEntity.HasOne(x => x.ToWallet);
            invoiceEntity.HasOne(x => x.Status);
        }
    }
}
