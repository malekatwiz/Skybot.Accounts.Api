using System;
using System.Threading.Tasks;
using Skybot.Accounts.Api.Data;
using Skybot.Accounts.Api.Models;

namespace Skybot.Accounts.Api.Services.Accounts
{
    public class AccountService : IAccountService
    {
        private readonly IAccountsRepository _accountRepository;

        public AccountService(IAccountsRepository accountRepository)
        {
            _accountRepository = accountRepository;
        }

        public Task<UserAccount> GetAccountByPhoneNumber(string phoneNumber)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserAccount> GeyById(Guid id)
        {
            throw new System.NotImplementedException();
        }

        public Task<UserAccount> NewAccount(UserAccountModel model)
        {
            return _accountRepository.CreateAccount(new UserAccount
            {
                PhoneNumber = model.PhoneNumber
            });
        }
    }
}
