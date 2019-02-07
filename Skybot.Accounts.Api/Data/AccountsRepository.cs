using System;
using System.Threading.Tasks;
using Skybot.Accounts.Api.Settings;

namespace Skybot.Accounts.Api.Data
{
    public class AccountsRepository : RepositoryBase<UserAccount>, IAccountsRepository
    {
        public AccountsRepository(ISettings settings) : base(settings, "Accounts")
        {
        }

        public UserAccount GetByPhoneNumber(string phoneNumber)
        {
            return GetBy(x => x.PhoneNumber.Equals(phoneNumber, StringComparison.InvariantCultureIgnoreCase));
        }

        public Task<UserAccount> Create(UserAccount account)
        {
            return AddAsync(account);
        }

        public override async Task UpdateAsync(UserAccount userAccount)
        {
            var document = GetByPhoneNumber(userAccount.PhoneNumber);

            document.PhoneNumber = userAccount.PhoneNumber;
            document.Name = userAccount.Name;
            document.AccessCode = userAccount.AccessCode;
            document.AccessCodeExpiry = userAccount.AccessCodeExpiry;

            await DocumentClient.UpsertDocumentAsync(CollectionUri, document);
        }
    }
}
