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

        public UserAccount GetByPhoneNumber(string phoneNumber)
        {
            return _accountRepository.GetByPhoneNumber(phoneNumber);
        }

        public Task<UserAccount> Gey(Guid id)
        {
            return _accountRepository.Get(id);
        }

        public Task<UserAccount> New(UserAccountModel model)
        {
            return _accountRepository.Create(new UserAccount
            {
                PhoneNumber = model.PhoneNumber
            });
        }
    }
}
