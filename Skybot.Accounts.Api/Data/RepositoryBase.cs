using System;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Skybot.Accounts.Api.Settings;

namespace Skybot.Accounts.Api.Data
{
    public class RepositoryBase : IRepository
    {
        protected DocumentClient DocumentClient;
        protected Uri AccountsCollectionUri;

        public RepositoryBase(ISettings settings)
        {
            DocumentClient = new DocumentClient(settings.SkybotDbEndpoint, settings.SkybotDbAuthKey);
            AccountsCollectionUri =
                UriFactory.CreateDocumentCollectionUri(settings.SkybotDbId, settings.SkybotAccountsCollectionId);
        }

        public async Task<UserAccount> Add(UserAccount account)
        {
            var document = await DocumentClient.CreateDocumentAsync(AccountsCollectionUri, account);

            return (dynamic)document.Resource;
        }
    }
}
