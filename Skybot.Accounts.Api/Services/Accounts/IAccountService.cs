using System;
using System.Threading.Tasks;
using Skybot.Accounts.Api.Data;
using Skybot.Accounts.Api.Models;

namespace Skybot.Accounts.Api.Services.Accounts
{
    public interface IAccountService
    {
        Task<UserAccount> New(UserAccountModel model);
        UserAccount GetByPhoneNumber(string phoneNumber);
        Task<UserAccount> Get(Guid id);
        Task<string> GenerateCode(Guid id);
        bool ValidateToken(string phoneNumber, string accessCode);
    }
}
