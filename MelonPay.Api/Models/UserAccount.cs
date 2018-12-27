using MelonPay.Common.Entities;
using System;

namespace MelonPay.Api.Models
{
    public class UserAccount
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public int CardHolderId { get; set; }
        public CardHolder CardHolder { get; set; }
    }
}
