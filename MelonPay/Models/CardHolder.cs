namespace MelonPay.Models
{
    public class CardHolder
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string MiddleName { get; set; }
        public string LastName { get; set; }
        public Gender? Gender { get; set; }
        public Wallet[] Wallets { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
