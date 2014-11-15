using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManager.Repository
{
    public class SampleBookRepository : IBookRepository
    {
        public IEnumerable<Book> ListAll()
        {
            return new List<Book> { new Book { Title = "ASP.net" }, new Book { Title = "Python" } };
        }

        public void Add(Book book)
        {
            return;
        }
    }
}