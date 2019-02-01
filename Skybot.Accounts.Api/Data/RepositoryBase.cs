using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents.Client;
using Skybot.Accounts.Api.Settings;

namespace Skybot.Accounts.Api.Data
{
    public class RepositoryBase<T> : IRepository<T>
    {
        protected DocumentClient DocumentClient;
        protected Uri CollectionUri;

        private readonly string _skybotDatabaseId;
        private readonly string _collectionId;

        public RepositoryBase(ISettings settings, string collectionId)
        {
            _skybotDatabaseId = settings.SkybotDbId;
            _collectionId = collectionId;

            DocumentClient = new DocumentClient(settings.SkybotDbEndpoint, settings.SkybotDbAuthKey);
            CollectionUri = UriFactory.CreateDocumentCollectionUri(settings.SkybotDbId, _collectionId);
        }

        public async Task<T> Add(T document)
        {
            var savedDocument = await DocumentClient.CreateDocumentAsync(CollectionUri, document);

            return (dynamic)savedDocument.Resource;
        }

        public async Task<T> Get(Guid id)
        {
            var document = await DocumentClient.ReadDocumentAsync<T>(UriFactory.CreateDocumentUri(_skybotDatabaseId,
                    _collectionId, id.ToString()));

            return document.Document;
        }

        public T GetBy(Func<T, bool> func)
        {
            return DocumentClient.CreateDocumentQuery<T>(CollectionUri)
                .Where(func).AsEnumerable().FirstOrDefault();
        }
    }
}
