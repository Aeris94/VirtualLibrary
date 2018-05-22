# VirtualLibary

Virtual Library is a simple program that enables you to keep track of books in library.
Each book has its unique identyfier, title, author, ISBN number, date that it was last borrowed on,
and name of the person who borrowed it.

Program enables user to:
- read and save catalogue to a XML or JSON file
- add a book to a catalogue
- remove book from a catalogue
- borrow a book
- return a book
- search for a book by title, author or ISBN
- search for books that were not borrowed for past x weeks
- display a list of people with borrowed books

## Running a program

In order to run a program, you must specify absolute file path to a XML or JSON file with list of books as a command line parameter.
If you wish to test a program in Visual Studio, you can use test.json or test.xml files from this repository. In
order to do this, add file path to Project -> Properties -> Debug -> Command line arguments.
