using System;
using Microsoft.Azure.Documents.Client;
using System.Linq;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;

namespace LibraryManager.Repository
{
    public class DocumentDBRepository
    {
        private Database _database;
        private DocumentClient _client;
        private DocumentCollection _collection;

        protected  DocumentClient Client { get { return _client; } }
        protected DocumentCollection Collection { get { return _collection; } }

        public DocumentDBRepository(string databaseName, string collectionName)
        {
            var configuration = new Configuration().AddJsonFile("config_keys.json");
            _client = new DocumentClient(new Uri(configuration.Get("Data:DocumentDB:Endpoint")), configuration.Get("Data:DocumentDB:AuthKey"));

            _database = _client.CreateDatabaseQuery().Where(d => d.Id == databaseName).AsEnumerable().FirstOrDefault();
            if (_database == null)
            {
                _database = new Database { Id = databaseName };
                _database = _client.CreateDatabaseAsync(new Database { Id = databaseName }).Result;
            }

            _collection = _client.CreateDocumentCollectionQuery(_database.CollectionsLink).Where(c => c.Id == collectionName).AsEnumerable().FirstOrDefault();
            if (_collection == null)
            {
                _collection = _client.CreateDocumentCollectionAsync(_database.CollectionsLink, new DocumentCollection { Id = collectionName }).Result;
            }
        }
    }
}