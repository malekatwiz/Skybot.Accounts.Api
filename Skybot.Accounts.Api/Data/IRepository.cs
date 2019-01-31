using System.Threading.Tasks;

namespace Skybot.Accounts.Api.Data
{
    public interface IRepository
    {
        Task<UserAccount> Add(UserAccount account);
    }
}
