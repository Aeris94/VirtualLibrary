using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualLibrary.Books;

namespace VirtualLibrary.Library
{
    public class LibraryService : ILibraryService
    {
        private readonly IBookRepository _bookRepository;

        public LibraryService(IBookRepository bookRepository)
        {
            _bookRepository = bookRepository;
        }

        public void AddBook(Book book)
        {
            _bookRepository.Add(book);
        }

        public void RemoveBook(Book book)
        {
            _bookRepository.Remove(book);
        }

        public void BorrowBook(Book book, string name)
        {
            book.BorrowerName = name;
            book.LastBorrowedOn = DateTime.Now;
            _bookRepository.Save();
        }

        public void ReturnBook(Book book)
        {
            book.BorrowerName = null;
            _bookRepository.Save();
        }

        public IEnumerable<Book> Search(SearchCriteria searchCriteria)
        {
            IEnumerable<Book> result = _bookRepository.Books;
            if (searchCriteria.Title != null)
            {
                result = result
                    .Where(b => b.Title == searchCriteria.Title)
                    .OrderBy(b => b.Title);
            }
            if (searchCriteria.Author != null)
            {
                result = result
                    .Where(b => b.Author == searchCriteria.Author)
                    .OrderBy(b => b.Author);
            }
            if (searchCriteria.Isbn != null)
            {
                result = result
                    .Where(b => b.Isbn == searchCriteria.Isbn)
                    .OrderBy(b => b.Isbn);
            }
            if (searchCriteria.Weeks != null)
            {
                var fromDate = DateTime.Today.AddDays(-7 * (double)searchCriteria.Weeks);
                result = result
                    .Where(b => b.BorrowerName == null)
                    .Where(b => b.LastBorrowedOn.HasValue)
                    .Where(b => b.LastBorrowedOn <= fromDate)
                    .OrderBy(b => b.LastBorrowedOn);
            }
            if (searchCriteria.Borrowed == true)
            {
                result = result
                    .Where(b => b.BorrowerName != null)
                    .OrderBy(b => b.BorrowerName);
            }
            if (searchCriteria.Borrowed == false)
            {
                result = result
                    .Where(b => b.BorrowerName == null)
                    .OrderBy(b => b.BorrowerName);
            }
            return result;
        }

        public IDictionary<string, int> UserSearch()
        {
            return _bookRepository.Books
                .Where(b => b.BorrowerName != null)
                .GroupBy(b => b.BorrowerName)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionary(Kv => Kv.Key, Kv => Kv.Count);
        }
    }
}
