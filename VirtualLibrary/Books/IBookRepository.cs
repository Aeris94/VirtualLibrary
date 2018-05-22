using System.Collections.Generic;

namespace VirtualLibrary.Books
{
    public interface IBookRepository
    {
        IEnumerable<Book> Books { get; }

        void Add(Book book);
        void Remove(Book book);
        void Save();
    }
}