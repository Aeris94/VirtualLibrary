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

        public IEnumerable<Book> Search(
           string title = null,
           string author = null,
           string isbn = null,
           int? weeks = null,
           bool? borrowed = null)
        {
            IEnumerable<Book> result = _bookRepository.Books;
            if (title != null)
            {
                result = result
                    .Where(b => b.Title == title)
                    .OrderBy(b => b.Title);
            }
            if (author != null)
            {
                result = result
                    .Where(b => b.Author == author)
                    .OrderBy(b => b.Author);
            }
            if (isbn != null)
            {
                result = result
                    .Where(b => b.Isbn == isbn)
                    .OrderBy(b => b.Isbn);
            }
            if (weeks != null)
            {
                var fromDate = DateTime.Today.AddDays(-7 * (double)weeks);
                result = result
                    .Where(b => b.BorrowerName == null)
                    .Where(b => b.LastBorrowedOn.HasValue)
                    .Where(b => b.LastBorrowedOn <= fromDate)
                    .OrderBy(b => b.LastBorrowedOn);
            }
            if (borrowed == true)
            {
                result = result
                    .Where(b => b.BorrowerName != null)
                    .OrderBy(b => b.BorrowerName);
            }
            if (borrowed == false)
            {
                return result = result
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
