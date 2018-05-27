using System.Collections.Generic;
using VirtualLibrary.Books;

namespace VirtualLibrary.Library
{
    public interface ILibraryPresenter
    {
        void DisplayBorrowedBooks(IDictionary<string, int> dictionary);
        void DisplayList(IEnumerable<Book> list);
    }
}