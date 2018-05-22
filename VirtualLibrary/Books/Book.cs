using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualLibrary.Books
{
    public class Book : IEquatable<Book>
    {
        public Guid BookId { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Isbn { get; set; }
        public DateTime? LastBorrowedOn { get; set; }
        public string BorrowerName { get; set; }

        public Book(Guid id)
        {
            BookId = id;
        }

        public Book()
        { }

        public override int GetHashCode()
        {
            return BookId.GetHashCode();
        }

        public override bool Equals(object obj)
        {
            Book other = obj as Book;
            if (ReferenceEquals(null, other))
                return false;

            return Equals(other);
        }

        public bool Equals(Book other)
        {
            if (ReferenceEquals(null, other))
                return false;

            return BookId.Equals(other.BookId);
        }
    }
}
