using System;
using Microsoft.Azure.Documents;

namespace Skybot.Accounts.Api.Data
{
    public class UserAccount
    {
        public Guid Id { get; set; }
        public string PhoneNumber { get; set; }
    }
}
