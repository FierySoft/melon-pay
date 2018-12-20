namespace MelonPay.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public int CardHolderId { get; set; }
        public CardHolder CardHolder { get; set; }
    }
}
