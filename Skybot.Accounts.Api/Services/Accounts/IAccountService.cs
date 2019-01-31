using System.Threading.Tasks;
using Skybot.Accounts.Api.Data;
using Skybot.Accounts.Api.Models;

namespace Skybot.Accounts.Api.Services.Accounts
{
    public interface IAccountService
    {
        Task<UserAccount> NewAccount(UserAccountModel model);
        Task<UserAccount> GetAccountByPhoneNumber(string phoneNumber);
    }
}
