using System.Linq;
using System.Collections.Generic;
using Microsoft.Azure.Documents.Linq;

namespace LibraryManager.Repository
{
    public class BookRepository : DocumentDBRepository, IBookRepository
    {
        public BookRepository() : base("LibraryManager", "LibraryCollection") {}

        public async void Add(Book book)
        {
            await Client.CreateDocumentAsync(Collection.DocumentsLink, book);
        }

        public IEnumerable<Book> ListAll()
        {
            var books = Client.CreateDocumentQuery<Book>(Collection.DocumentsLink).Select(x => x);
            return books.ToList();
        }
    }
}