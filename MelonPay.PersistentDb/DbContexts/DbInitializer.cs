using System;
using System.Linq;
using MelonPay.Common.Entities;

namespace MelonPay.Persisted.DbContexts
{
    public static class DbInitializer
    {
        public static void Initialize(MelonPayDbContext context)
        {
            // Check database exists
            context.Database.EnsureCreated();

            if (!context.Currencies.Any())
            {
                context.Currencies.AddRange(new Currency[]
                {
                    new Currency { Code = "RUB", Name = "Рубль" },
                    new Currency { Code = "USD", Name = "Доллар" },
                    new Currency { Code = "EUR", Name = "Евро" }
                });
                context.SaveChanges();
            }

            if (!context.InvoiceStatuses.Any())
            {
                context.InvoiceStatuses.AddRange(new InvoiceStatus[]
                {
                    new InvoiceStatus { Code = "New", Name = "Новый" },
                    new InvoiceStatus { Code = "Declined", Name = "Отклонен" },
                    new InvoiceStatus { Code = "Payed", Name = "Оплачен" },
                    new InvoiceStatus { Code = "Error", Name = "Ошибка" }
                });
                context.SaveChanges();
            }

            if (!context.CardHolders.Any())
            {
                context.CardHolders.AddRange(new CardHolder[]
                {
                    new CardHolder { FirstName = "Иван", MiddleName = "Иванович", LastName = "Иванов", Gender = Gender.Male, IsDeleted = false },
                    new CardHolder { FirstName = "Петр", MiddleName = "Петрович", LastName = "Петров", Gender = Gender.Male, IsDeleted = false },
                    new CardHolder { FirstName = "Мария", MiddleName = "Алексеевна", LastName = "Собакина", Gender = Gender.Female, IsDeleted = true }
                });
                context.SaveChanges();
            }

            if (!context.Accounts.Any())
            {
                context.Accounts.AddRange(new Account[]
                {
                    new Account { UserName = "+79010012234", Password = "gfhjkm123", CardHolderId = 1, CreatedAt = new DateTime(2010, 9, 21), IsDeleted = false },
                    new Account { UserName = "+79939993393", Password = "gfhjkm123", CardHolderId = 2, CreatedAt = new DateTime(2017, 3, 22), IsDeleted = false },
                    new Account { UserName = "+79511654586", Password = "gfhjkm123", CardHolderId = 3, CreatedAt = new DateTime(2015, 8, 2), IsDeleted = true }
                });
                context.SaveChanges();
            }

            if (!context.Wallets.Any())
            {
                context.Wallets.AddRange(new WalletPrivate[]
                {
                    new WalletPrivate { CurrencyId = 1, CardHolderId = 1, Balance = 5000m, IsDeleted = false },
                    new WalletPrivate { CurrencyId = 2, CardHolderId = 1, Balance = 100m, IsDeleted = false },
                    new WalletPrivate { CurrencyId = 1, CardHolderId = 2, Balance = 100m, IsDeleted = false },
                    new WalletPrivate { CurrencyId = 3, CardHolderId = 2, Balance = 1000m, IsDeleted = false }
                });
                context.SaveChanges();
            }

            if (!context.Invoices.Any())
            {
                context.Invoices.AddRange(new Invoice[]
                {
                    new Invoice { Amount = 10m, Comment = "Подарок на ДР", CreatedAt = DateTime.Now, FromWalletId = 1, ToWalletId = 3, StatusId = 1 }
                });
                context.SaveChanges();
            }
        }
    }
}
