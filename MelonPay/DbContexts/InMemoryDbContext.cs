using System;
using System.Collections.Generic;
using MelonPay.Entities;

namespace MelonPay.DbContexts
{
    public class InMemoryDbContext
    {
        public ICollection<Currency> Currencies = new List<Currency>()
        {
            new Currency { Id = 1, Code = "RUB", Name = "Рубль" },
            new Currency { Id = 2, Code = "USD", Name = "Доллар" },
            new Currency { Id = 3, Code = "EUR", Name = "Евро" }
        };

        public ICollection<InvoiceStatus> InvoiceStatuses = new List<InvoiceStatus>()
        {
            new InvoiceStatus { Id = 1, Code = "New", Name = "Новый" },
            new InvoiceStatus { Id = 2, Code = "Declined", Name = "Отклонен" },
            new InvoiceStatus { Id = 3, Code = "Payed", Name = "Оплачен" }
        };

        public ICollection<CardHolder> CardHolders = new List<CardHolder>()
        {
            new CardHolder { Id = 1, FirstName = "Иван", MiddleName = "Иванович", LastName = "Иванов", Gender = Gender.Male, IsDeleted = false },
            new CardHolder { Id = 2, FirstName = "Петр", MiddleName = "Петрович", LastName = "Петров", Gender = Gender.Male, IsDeleted = false },
            new CardHolder { Id = 3, FirstName = "Мария", MiddleName = "Алексеевна", LastName = "Собакина", Gender = Gender.Female, IsDeleted = true }
        };

        public ICollection<Account> Accounts = new List<Account>()
        {
            new Account { Id = 1, UserName = "+79010012234", Password = "gfhjkm123", CardHolderId = 1, CreatedAt = new DateTime(2010, 9, 21), IsDeleted = false },
            new Account { Id = 2, UserName = "+79939993393", Password = "gfhjkm123", CardHolderId = 2, CreatedAt = new DateTime(2017, 3, 22), IsDeleted = false },
            new Account { Id = 3, UserName = "+79511654586", Password = "gfhjkm123", CardHolderId = 3, CreatedAt = new DateTime(2015, 8, 2), IsDeleted = true }
        };

        public ICollection<WalletPrivate> Wallets = new List<WalletPrivate>()
        {
            new WalletPrivate { Id = 1, CurrencyId = 1, CardHolderId = 1, Balance = 5000m, IsDeleted = false },
            new WalletPrivate { Id = 2, CurrencyId = 2, CardHolderId = 1, Balance = 100m, IsDeleted = false },
            new WalletPrivate { Id = 3, CurrencyId = 1, CardHolderId = 2, Balance = 100m, IsDeleted = false },
            new WalletPrivate { Id = 4, CurrencyId = 3, CardHolderId = 2, Balance = 1000m, IsDeleted = false }
        };

        public ICollection<Invoice> Invoices = new List<Invoice>()
        {
            new Invoice { Id = 1, Amount = 10m, Comment = "Подарок на ДР", CreatedAt = DateTime.Now, FromWalletId = 1, ToWalletId = 3, StatusId = 1 }
        };
    }
}
