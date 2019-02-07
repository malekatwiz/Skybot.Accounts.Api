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
        private readonly string _partitionKey;

        public RepositoryBase(ISettings settings, string collectionId, string partitionKey)
        {
            _skybotDatabaseId = settings.SkybotDbId;
            _collectionId = collectionId;
            _partitionKey = partitionKey;

            DocumentClient = new DocumentClient(settings.SkybotDbEndpoint, settings.SkybotDbAuthKey);
            CollectionUri = UriFactory.CreateDocumentCollectionUri(settings.SkybotDbId, _collectionId);
        }

        public async Task<T> Add(T document)
        {
            var savedDocument = await DocumentClient.CreateDocumentAsync(CollectionUri, document);

            return (dynamic)savedDocument.Resource;
        }

        public virtual async Task<T> Get(Guid id)
        {
            //TODO: Fix error here.
            var document = await DocumentClient.ReadDocumentAsync<T>(UriFactory.CreateDocumentUri(_skybotDatabaseId,
                    _collectionId, id.ToString()));

            return document.Document;
        }

        public T GetBy(Func<T, bool> func)
        {
            return DocumentClient.CreateDocumentQuery<T>(CollectionUri)
                .Where(func).AsEnumerable().FirstOrDefault();
        }

        public virtual async Task UpdateAsync(T item)
        {
            await DocumentClient.UpsertDocumentAsync(CollectionUri, item);
        }
    }
}
