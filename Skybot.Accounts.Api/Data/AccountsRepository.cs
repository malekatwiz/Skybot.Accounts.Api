using System;
using System.Threading.Tasks;
using Skybot.Accounts.Api.Settings;

namespace Skybot.Accounts.Api.Data
{
    public class AccountsRepository : RepositoryBase<UserAccount>, IAccountsRepository
    {
        public AccountsRepository(ISettings settings) : base(settings, "Accounts")
        {
        }

        public UserAccount GetByPhoneNumber(string phoneNumber)
        {
            return GetBy(x => x.PhoneNumber.Equals(phoneNumber, StringComparison.InvariantCultureIgnoreCase));
        }

        public Task<UserAccount> Create(UserAccount account)
        {
            return Add(account);
        }
    }
}
