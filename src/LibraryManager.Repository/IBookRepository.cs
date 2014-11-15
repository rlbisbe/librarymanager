using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LibraryManager.Repository
{
    public interface IBookRepository
    {
        IEnumerable<Book> ListAll();
        void Add(Book book);
    }
}