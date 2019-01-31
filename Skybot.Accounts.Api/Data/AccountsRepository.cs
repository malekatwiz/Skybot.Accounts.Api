using System.Threading.Tasks;
using Skybot.Accounts.Api.Settings;

namespace Skybot.Accounts.Api.Data
{
    public class AccountsRepository : RepositoryBase, IAccountsRepository
    {
        public AccountsRepository(ISettings settings) : base(settings)
        {
        }

        public Task<UserAccount> GetAccountByPhoneNumber(string phoneNumber)
        {
            return null;
        }

        public Task<UserAccount> CreateAccount(UserAccount account)
        {
            return Add(account);
        }
    }
}
