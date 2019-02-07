using System;

namespace Skybot.Accounts.Api.Data
{
    public class UserAccount
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
        public string Name { get; set; }
        public string AccessCode { get; set; }
        public DateTime AccessCodeExpiry { get; set; }
    }
}
