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
            if(!list.Any())
            {
                Console.WriteLine("No results.");
                return;
            }

            int index = 1;
            StringBuilder result = new StringBuilder();

            foreach (var item in list)
            {
                result.Append($"\nResult {index} ");
                result.Append(RecordToString(item));
                index++;
            }

            Console.WriteLine(result.ToString());
        }

        public void DisplayBorrowedBooks(IDictionary<string, int> dictionary)
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
