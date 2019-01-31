using System.Threading.Tasks;
using Skybot.Accounts.Api.Models;

namespace Skybot.Accounts.Api.Data
{
    public class AccountsRepository : IAccountsRepository
    {
        public Task<UserAccountModel> GetAccountByPhoneNumber(string phoneNumber)
        {
            return null;
        }
    }
}
