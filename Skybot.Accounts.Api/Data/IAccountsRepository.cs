using System.Threading.Tasks;

namespace Skybot.Accounts.Api.Data
{
    public interface IAccountsRepository
    {
        Task<UserAccount> GetAccountByPhoneNumber(string phoneNumber);

        Task<UserAccount> CreateAccount(UserAccount account);
    }
}
