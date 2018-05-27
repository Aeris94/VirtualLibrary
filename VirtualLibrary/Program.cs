using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using VirtualLibrary.Books;
using VirtualLibrary.Configurations;
using VirtualLibrary.Library;

namespace VirtualLibrary
{
    class Program
    {
        private static IBookRepository _bookRepository;
        private static ILibraryService _libraryService;
        private static ILibraryPresenter _libraryPresenter;

        static void Main(string[] args)
        {
            if (args.Length != 0)
            {
                Launching(args[0]);
            }
            else
            {
                Console.WriteLine("File path was not specified.");
                Exit();
            }

            Console.WriteLine("Welcome to a Virtual Libary!");
            Menu();
            return;
        }


        private static void Exit()
        {
            Console.WriteLine("Thank you for visiting VirtualLibary.");
            Environment.Exit(0);
        }

        private static void Launching(string args)
        {
            string filePath = args;
            if (!File.Exists(filePath))
            {
                Console.WriteLine("File does not exist.");
                Exit();
            }

            var fileType = filePath.Substring(filePath.LastIndexOf(".") + 1);
            if (fileType != "json" && fileType != "xml")
            {
                Console.WriteLine("Sorry, Virtual Libary does not support given file type.");
                Exit();
            }

            IConfiguration configuration = new ConsoleConfiguration()
            {
                FilePath = filePath,
            };

            if (fileType == "json")
                _bookRepository = new JsonBookRepository(configuration);
            else
                _bookRepository = new XmlBookRepository(configuration);

            _libraryService = new LibraryService(_bookRepository);
            _libraryPresenter = new ConsoleLibraryPresenter();

        }

        public static Dictionary<char, Action> menuOptions = new Dictionary<char, Action>
        {
            ['1'] = AddBook,
            ['2'] = RemoveBook,
            ['3'] = Search,
            ['4'] = BorrowBook,
            ['5'] = ReturnBook,
            ['6'] = ShowUsers,
            ['7'] = ShowAllBooks,
            ['q'] = Exit,
            ['Q'] = Exit
        };

        private static void Menu()
        {
            string strMenuOption;

            while(true)
            {
                Console.WriteLine("\nPlease choose an option: ");
                DisplayOptions();
                strMenuOption = Console.ReadLine();

                if(!char.TryParse(strMenuOption, out var menuOption))
                {
                    Console.WriteLine("There is no such option.");
                    continue;
                }

                if(!menuOptions.ContainsKey(menuOption))
                {
                    Console.WriteLine("There is no such option.");
                    continue;
                }

                menuOptions[menuOption]();
            }
        }

        private static void DisplayOptions()
        {
            Console.WriteLine(
                string.Format(
                " 1 - Add a book to libary catalog." +
                "\n 2 - Remove a book from libary catalog" +
                "\n 3 - Search for a book" +
                "\n 4 - Boorow a book." +
                "\n 5 - Return a book" +
                "\n 6 - Show list of people witch borrowed books" +
                "\n 7 - Show all books in libary" +
                "\n Q - Exit Virtual Libary"));
        }

        private static void AddBook()
        {
            Console.WriteLine("Enetr title: ");
            var title = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(title))
            {
                Console.WriteLine("Invalid title.");
                return;
            }

            Console.WriteLine("Enter author: ");
            var author = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(author))
            {
                Console.WriteLine("Invalid author.");
                return;
            }
            Console.WriteLine("Enter ISBN: ");
            var isbn = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(isbn))
            {
                Console.WriteLine("Invalid ISBN.");
                return;
            }

            var book = new Book(Guid.NewGuid())
            {
                Title = title,
                Author = author,
                Isbn = isbn,
            };
            _libraryService.AddBook(book);
            Console.WriteLine("Book was successfully added to libary catalog.");
        }

        private static void RemoveBook()
        {
            _libraryPresenter.DisplayList(_libraryService.Search(new SearchCriteria()));

            Console.WriteLine("Plese enter result number.");

            if (!int.TryParse(Console.ReadLine(), out var position))
            {
                Console.WriteLine("Invalid number.");
                return;
            }

            position--;
            var booksToRemove = _libraryService.Search(new SearchCriteria());
            if (position >= booksToRemove.Count() || position < 0)
            {
                Console.WriteLine("This book doesnt exist.");
                return;
            }

            var book = booksToRemove.ElementAt(position);
            _libraryService.RemoveBook(book);
            Console.WriteLine("Book was successfully removed.");
        }

        private static void Search()
        {
            Console.WriteLine("\nPlsese enter one or more of the following search criterium:" +
                "\n title," +
                "\n author, " +
                "\n ISBN " +
                "\n number of weeks sice books was last borrowed." +
                "\n\n Press enter to skip critierium.\n");

            Console.WriteLine("Plese enter title: ");
            var title = Console.ReadLine().Trim();
            Console.WriteLine("Plese enter author: ");
            var author = Console.ReadLine().Trim();
            Console.WriteLine("Plese enter ISBN: ");
            var isbn = Console.ReadLine().Trim();
            Console.WriteLine("Plese enter number of weeks since book was last borrowed: ");
            var strWeeks = Console.ReadLine().Trim();
            int? nullableWeeks = null;

            if (string.IsNullOrWhiteSpace(title))
                title = null;
            if (string.IsNullOrWhiteSpace(author))
                author = null;
            if (string.IsNullOrWhiteSpace(isbn))
                isbn = null;
            if (string.IsNullOrEmpty(strWeeks))
                nullableWeeks = null;
            if (int.TryParse(strWeeks, out var weeks) && weeks > 0)
            {
                nullableWeeks = weeks;
            }

            var result = _libraryService.Search(
                new SearchCriteria(
                    title: title, 
                    author: author,
                    isbn: isbn,
                    weeks: nullableWeeks));
            _libraryPresenter.DisplayList(result);
        }

        private static void BorrowBook()
        {
            Console.WriteLine("Available books: ");
            _libraryPresenter.DisplayList(_libraryService.Search(new SearchCriteria(borrowed: false)));

            Console.WriteLine("Plese enter a name: ");
            var name = Console.ReadLine().Trim();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Invalid argument.");
                return;
            }

            Console.WriteLine("Plese enter result number: ");
            if (!int.TryParse(Console.ReadLine(), out var position))
            {
                Console.WriteLine("Invalid argumnet.");
                return;
            }

            position--;
            var booksToBorrow = _libraryService.Search(new SearchCriteria(borrowed: false));
            if (position >= booksToBorrow.Count() || position < 0)
            {
                Console.WriteLine("This book doesnt exist.");
                return;
            }

            var book = booksToBorrow.ElementAt(position);
            _libraryService.BorrowBook(book, name);
            Console.WriteLine("Book was successfully borrowed.");
        }

        private static void ReturnBook()
        {
            Console.WriteLine("Borrowed books: ");
            _libraryPresenter.DisplayList(_libraryService.Search(new SearchCriteria(borrowed: true)));

            Console.WriteLine("Plese enter result number: ");
            if (!int.TryParse(Console.ReadLine(), out var position))
            {
                Console.WriteLine("Invalid argument.");
                return;
            }

            position--;
            var booksToReturn = _libraryService.Search(new SearchCriteria(borrowed: true));
            if (position >= booksToReturn.Count() || position < 0)
            {
                Console.WriteLine("This book doesnt exist.");
                return;
            }

            var book = booksToReturn.ElementAt(position);
            _libraryService.ReturnBook(book);
            Console.WriteLine("Book was successfully returned.");
        }

        private static void ShowUsers()
        {
            _libraryPresenter.DisplayBorrowedBooks(_libraryService.UserSearch());
        }

        private static void ShowAllBooks()
        {
            _libraryPresenter.DisplayList(_libraryService.Search(new SearchCriteria()));
        }
    }
}
