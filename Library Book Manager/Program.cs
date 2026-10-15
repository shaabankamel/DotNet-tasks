
using System;
using System.Collections.Generic;
using System.Linq;

internal class Program
{
    private static void Main(string[] args)
    {
        Library library = new Library();
        bool running = true;

        while (running)
        {
            Console.WriteLine("\n========== Library Book Manager ==========");
            Console.WriteLine("1. Add Book");
            Console.WriteLine("2. Remove Book");
            Console.WriteLine("3. Display All Books");
            Console.WriteLine("4. Borrow Book");
            Console.WriteLine("5. Return Book");
            Console.WriteLine("6. Search by Title");
            Console.WriteLine("7. Search by Author");
            Console.WriteLine("0. Exit");
            Console.Write("Choose an option: ");

            string? choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Enter title: ");
                    string title = Console.ReadLine() ?? "";

                    Console.Write("Enter author: ");
                    string author = Console.ReadLine() ?? "";

                    Console.Write("Enter ISBN: ");
                    string isbn = Console.ReadLine() ?? "";

                    library.AddBook(new Book(title, author, isbn));
                    break;

                case "2":
                    Console.Write("Enter ISBN of the book to remove: ");
                    library.RemoveBook(Console.ReadLine() ?? "");
                    break;

                case "3":
                    library.DisplayBooks();
                    break;

                case "4":
                    Console.Write("Enter book title: ");
                    string borrowTitle = Console.ReadLine() ?? "";

                    Console.Write("Enter borrower name: ");
                    string borrowerName = Console.ReadLine() ?? "";

                    library.BorrowBook(borrowTitle, borrowerName);
                    break;

                case "5":
                    Console.Write("Enter book title: ");
                    library.ReturnBook(Console.ReadLine() ?? "");
                    break;

                case "6":
                    Console.Write("Enter book title: ");
                    library.SearchByTitle(Console.ReadLine() ?? "");
                    break;

                case "7":
                    Console.Write("Enter author name: ");
                    library.SearchByAuthor(Console.ReadLine() ?? "");
                    break;

                case "0":
                    running = false;
                    Console.WriteLine("Goodbye!");
                    break;

                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}

class Book
{
    public string Title { get; set; }
    public string Author { get; set; }
    public string ISBN { get; set; }
    public bool IsAvailable { get; private set; } = true;
    public string? BorrowerName { get; private set; }

    public Book(string title, string author, string isbn)
    {
        Title = title;
        Author = author;
        ISBN = isbn;
    }

    public bool Borrow(string borrowerName)
    {
        if (!IsAvailable)
        {
            Console.WriteLine($"Sorry, '{Title}' is currently not available.");
            return false;
        }

        IsAvailable = false;
        BorrowerName = borrowerName;

        Console.WriteLine($"'{Title}' was borrowed by {borrowerName}.");
        return true;
    }

    public bool Return()
    {
        if (IsAvailable)
        {
            Console.WriteLine($"'{Title}' is not currently borrowed.");
            return false;
        }

        IsAvailable = true;
        BorrowerName = null;

        Console.WriteLine($"'{Title}' has been returned.");
        return true;
    }
}

class Library
{
    private readonly List<Book> books = new List<Book>();

    public void AddBook(Book book)
    {
        if (string.IsNullOrWhiteSpace(book.Title) ||
            string.IsNullOrWhiteSpace(book.Author) ||
            string.IsNullOrWhiteSpace(book.ISBN))
        {
            Console.WriteLine("Title, author, and ISBN are required.");
            return;
        }

        if (books.Any(b => b.ISBN.Equals(
            book.ISBN, StringComparison.OrdinalIgnoreCase)))
        {
            Console.WriteLine("A book with this ISBN already exists.");
            return;
        }

        books.Add(book);
        Console.WriteLine($"'{book.Title}' added successfully.");
    }

    public void RemoveBook(string isbn)
    {
        Book? book = books.FirstOrDefault(b =>
            b.ISBN.Equals(isbn, StringComparison.OrdinalIgnoreCase));

        if (book == null)
        {
            Console.WriteLine("Book not found.");
            return;
        }

        if (!book.IsAvailable)
        {
            Console.WriteLine("Cannot remove a borrowed book.");
            return;
        }

        books.Remove(book);
        Console.WriteLine($"'{book.Title}' removed successfully.");
    }

    public void DisplayBooks()
    {
        if (books.Count == 0)
        {
            Console.WriteLine("The library is empty.");
            return;
        }

        Console.WriteLine("\n========== Library Report ==========");

        foreach (Book book in books)
        {
            DisplayBookDetails(book);
        }

        Console.WriteLine($"Total books: {books.Count}");
    }

    public void BorrowBook(string title, string borrowerName)
    {
        if (string.IsNullOrWhiteSpace(borrowerName))
        {
            Console.WriteLine("Borrower name is required.");
            return;
        }

        List<Book> matches = FindByTitle(title);

        if (matches.Count == 0)
        {
            Console.WriteLine($"Book '{title}' was not found.");
            return;
        }

        if (matches.Count > 1)
        {
            Console.WriteLine("Multiple books have this title. Please use a unique title.");
            return;
        }

        matches[0].Borrow(borrowerName);
    }

    public void ReturnBook(string title)
    {
        List<Book> matches = FindByTitle(title);

        if (matches.Count == 0)
        {
            Console.WriteLine($"Book '{title}' was not found.");
            return;
        }

        if (matches.Count > 1)
        {
            Console.WriteLine("Multiple books have this title. Please use a unique title.");
            return;
        }

        matches[0].Return();
    }

    public void SearchByTitle(string title)
    {
        List<Book> results = FindByTitle(title);

        DisplaySearchResults(results, $"title '{title}'");
    }

    public void SearchByAuthor(string author)
    {
        List<Book> results = books
            .Where(b => b.Author.Contains(
                author, StringComparison.OrdinalIgnoreCase))
            .ToList();

        DisplaySearchResults(results, $"author '{author}'");
    }

    private List<Book> FindByTitle(string title)
    {
        return books
            .Where(b => b.Title.Equals(
                title, StringComparison.OrdinalIgnoreCase))
            .ToList();
    }

    private void DisplaySearchResults(List<Book> results, string description)
    {
        if (results.Count == 0)
        {
            Console.WriteLine($"No books found for {description}.");
            return;
        }

        foreach (Book book in results)
        {
            DisplayBookDetails(book);
        }
    }

    private void DisplayBookDetails(Book book)
    {
        Console.WriteLine("------------------------------------");
        Console.WriteLine($"Title: {book.Title}");
        Console.WriteLine($"Author: {book.Author}");
        Console.WriteLine($"ISBN: {book.ISBN}");
        Console.WriteLine($"Status: {(book.IsAvailable ? "Available" : "Borrowed")}");
        Console.WriteLine($"Borrower: {book.BorrowerName ?? "None"}");
    }
}
