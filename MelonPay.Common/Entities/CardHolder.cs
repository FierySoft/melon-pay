using System.Collections.Generic;
using System.Linq;

namespace MelonPay.Common.Entities
{
    public class CardHolder
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public bool IsDeleted { get; set; }
        public IEnumerable<Wallet> Wallets { get; set; }

        public string FullName => $"{FirstName} {MiddleName} {LastName}";
        public string ShortName => $"{LastName} {FirstName?.First()}.{MiddleName?.First()}.";
    }

    public enum Gender
    {
        Male,
        Female
    }
}
