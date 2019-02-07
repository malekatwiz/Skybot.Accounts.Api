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

        public Task<UserAccount> Get(string id)
        {
            return _accountRepository.GetAsync(id);
        }

        public Task<UserAccount> New(UserAccountModel model)
        {
            return _accountRepository.Create(new UserAccount
            {
                PhoneNumber = model.PhoneNumber,
                Name = model.Name
            });
        }

        public async Task<string> GenerateAccessCode(string id)
        {
            var userAccount = await Get(id);

            if (userAccount != null)
            {
                userAccount.AccessCode = GenerateAccessCode(123456, 999999).ToString();
                userAccount.AccessCodeExpiry = DateTime.Now.AddMinutes(30);

                await _accountRepository.ReplaceAsync(userAccount.Id, userAccount);

                return userAccount.AccessCode;
            }

            return string.Empty;
        }

        public bool ValidateAccessCode(string phoneNumber, string accessCode)
        {
            var userAccount = GetByPhoneNumber(phoneNumber);
            return accessCode.Equals(userAccount.AccessCode, StringComparison.InvariantCulture);
        }

        private static int GenerateAccessCode(int min, int max)
        {
            var random = new Random();
            return random.Next(min, max);
        }
    }
}
