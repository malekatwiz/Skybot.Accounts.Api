using System.Threading.Tasks;

namespace Skybot.Accounts.Api.Data
{
    public interface IAccountsRepository : IRepository<UserAccount>
    {
        UserAccount GetByPhoneNumber(string phoneNumber);

        Task<UserAccount> Create(UserAccount account);
    }
}
