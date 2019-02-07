using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Client;
using Skybot.Accounts.Api.Settings;

namespace Skybot.Accounts.Api.Data
{
    public class RepositoryBase<T> : IRepository<T>
    where T: class
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

        public async Task<T> AddAsync(T document)
        {
            var savedDocument = await DocumentClient.CreateDocumentAsync(CollectionUri, document);

            return (dynamic)savedDocument.Resource;
        }

        public virtual async Task<T> GetAsync(string id, string partitionKey = null)
        {
            var document = await DocumentClient.ReadDocumentAsync<T>(
                UriFactory.CreateDocumentUri(_skybotDatabaseId, _collectionId, partitionKey ?? id), 
                new RequestOptions{PartitionKey = new PartitionKey(id) });

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

        public virtual async Task ReplaceAsync(string id, T item)
        {
            await DocumentClient.ReplaceDocumentAsync(
                UriFactory.CreateDocumentUri(_skybotDatabaseId, _collectionId, id), item);
        }
    }
}
