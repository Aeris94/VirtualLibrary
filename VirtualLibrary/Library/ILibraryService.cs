using System.Collections.Generic;
using VirtualLibrary.Books;

namespace VirtualLibrary.Library
{
    public interface ILibraryService
    {
        void AddBook(Book book);
        void BorrowBook(Book book, string name);
        void RemoveBook(Book book);
        void ReturnBook(Book book);
        IEnumerable<Book> Search(SearchCriteria searchCriteria);
        IDictionary<string, int> UserSearch();
    }
}
