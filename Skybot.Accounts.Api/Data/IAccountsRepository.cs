using System.Threading.Tasks;
using Skybot.Accounts.Api.Models;

namespace Skybot.Accounts.Api.Data
{
    public interface IAccountsRepository
    {
        Task<UserAccountModel> GetAccountByPhoneNumber(string phoneNumber);
    }
}
