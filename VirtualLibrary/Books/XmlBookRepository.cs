using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using VirtualLibrary.Configurations;

namespace VirtualLibrary.Books
{
    public class XmlBookRepository : IBookRepository
    {
        private readonly string FilePath;

        private readonly List<Book> _books;
        public IEnumerable<Book> Books
        {
            get { return _books; }
        }

        public XmlBookRepository(IConfiguration configuration)
        {
            FilePath = configuration.FilePath;
            if (File.Exists(FilePath))
            {
                XmlSerializer xmlser = new XmlSerializer(typeof(List<Book>));
                StreamReader rfile = new StreamReader(FilePath);
                _books = (List<Book>)xmlser.Deserialize(rfile);
                rfile.Close();
            }
            else
            {
                _books = new List<Book>();
            }
        }

        public void Save()
        {
            XmlSerializer xmlser = new XmlSerializer(typeof(List<Book>));
            StreamWriter wfile = new StreamWriter(FilePath);
            xmlser.Serialize(wfile, Books);
            wfile.Close();
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
