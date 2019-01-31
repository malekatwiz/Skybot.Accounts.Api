using Microsoft.Azure.Documents;

namespace Skybot.Accounts.Api.Data
{
    public class UserAccount : Resource
    {
        public string PhoneNumber { get; set; }
    }
}
