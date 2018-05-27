
namespace VirtualLibrary.Library
{
    public class SearchCriteria
    {
        public string Title { get; }
        public string Author { get; }
        public string Isbn { get; }
        public int? Weeks { get; }
        public bool? Borrowed { get; }

        public SearchCriteria(
            string title = null,
            string author = null,
            string isbn = null,
            int? weeks = null,
            bool? borrowed = null
            )
        {
            this.Title = title;
            this.Author = author;
            this.Isbn = isbn;
            this.Weeks = weeks;
            this.Borrowed = borrowed;
        }
    }
}
