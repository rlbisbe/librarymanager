using Microsoft.Azure.Documents.Client;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.Framework.ConfigurationModel;
using Microsoft.Azure.Documents;
using Microsoft.Azure.Documents.Linq;
using System.Threading.Tasks;

namespace LibraryManager.Repository
{
    public class BookRepository : IBookRepository
    {
        private Database _database;
        private DocumentClient _client;
        private DocumentCollection _collection;
        private string _databaseId = "LibraryManager";
        private string _collectionId = "LibraryCollection";

        public BookRepository()
        {
            var configuration = new Configuration().AddJsonFile("config_data.json");
            _client = new DocumentClient(new Uri(configuration.Get("Data:DocumentDB:Endpoint")), configuration.Get("Data:DocumentDB:AuthKey"));

            _database = _client.CreateDatabaseQuery().Where(d => d.Id == _databaseId).AsEnumerable().FirstOrDefault();
            if (_database == null)
            {
                _database = new Database { Id = "LibraryManager" };
                _database = _client.CreateDatabaseAsync(new Database { Id = "LibraryManager" }).Result;
            }

            _collection = _client.CreateDocumentCollectionQuery(_database.CollectionsLink).Where(c => c.Id == "LibraryCollection").AsEnumerable().FirstOrDefault();
            if (_collection == null)
            {
                _collection = _client.CreateDocumentCollectionAsync(_database.CollectionsLink, new DocumentCollection { Id = _collectionId }).Result;
            }
        }

        public async void Add(Book book)
        {
            await _client.CreateDocumentAsync(_collection.DocumentsLink, book);
        }

        public IEnumerable<Book> ListAll()
        {
            var books = _client.CreateDocumentQuery<Book>(_collection.DocumentsLink).Select(x => x);
            return books.ToList();
        }
    }
}