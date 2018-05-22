using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using VirtualLibrary.Books;

namespace VirtualLibrary.Library
{
    public class ConsoleLibraryPresenter : ILibraryPresenter
    {
        public void DisplayList(IEnumerable<Book> list)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < list.Count(); i++)
            {
                result.Append($"\nResult {i + 1} " + RecordToString(list.ElementAt(i)));
            }
            Console.WriteLine(result.Length == 0 ? "No results." : result.ToString());
        }

        public void DiplayDictionary(IDictionary<string, int> dictionary)
        {
            StringBuilder result = new StringBuilder();

            foreach (var item in dictionary)
            {
                result.AppendLine($"{item.Key} - {item.Value}" + ((item.Value == 1) ? " book" : " books"));
            }
            Console.WriteLine((result.Length == 0) ? "No results." : result.ToString());
        }

        private string RecordToString(Book item)
        {
            return string.Format(
                   $"\n  Title: {item.Title}" +
                   $"\n  Author: {item.Author}" +
                   $"\n  Isbn: {item.Isbn}" +
                   $"\n  Last borrowed on: {item.LastBorrowedOn}" +
                   $"\n  Borrower name: {item.BorrowerName}" +
                   "\n");
        }
    }
}
