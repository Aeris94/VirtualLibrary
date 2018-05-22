using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualLibrary.Configurations;

namespace VirtualLibrary.Books
{
    public class JsonBookRepository : IBookRepository
    {
        private readonly string FilePath;

        private readonly List<Book> _books;
        public IEnumerable<Book> Books
        {
            get { return _books; }
        }

        public JsonBookRepository(IConfiguration configuration)
        {
            FilePath = configuration.FilePath;
            if (File.Exists(FilePath))
            {
                _books = JsonConvert.DeserializeObject<List<Book>>(
                    File.ReadAllText(
                        FilePath,
                        Encoding.UTF8));
            }
            else
            {
                _books = new List<Book>();
            }
        }

        public void Save()
        {
            File.WriteAllText(
                FilePath,
                JsonConvert.SerializeObject(
                    Books,
                    Formatting.Indented),
                Encoding.UTF8);
        }

        public void Add(Book book)
        {
            if (!_books.Contains(book))
                _books.Add(book);
            Save();
        }

        public void Remove(Book book)
        {
            _books.Remove(book);
            Save();
        }
    }
}
