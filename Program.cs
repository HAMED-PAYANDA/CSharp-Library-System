using System;
using System.IO;

class LibraryManager
{
    // Array of books (max 5)
    static string[] books = new string[5];

    // Checked-out flags for each book
    static bool[] checkedOut = new bool[5];

    // Borrow tracking
    static int borrowedCount = 0;
    const int borrowLimit = 3;

    // File path for saving/loading
    static string filePath = "library.txt";

    // ANSI color codes
    const string Red = "\u001b[31m";
    const string Green = "\u001b[32m";
    const string Yellow = "\u001b[33m";
    const string Cyan = "\u001b[36m";
    const string Reset = "\u001b[0m";

    static void Main()
    {
        LoadFromFile(); // Load saved books at startup

        while (true)
        {
            Console.WriteLine(Cyan + "\n===== Library Menu =====" + Reset);
            Console.WriteLine("1. Add a Book");
            Console.WriteLine("2. Remove a Book");
            Console.WriteLine("3. Display Books");
            Console.WriteLine("4. Search for a Book");
            Console.WriteLine("5. Borrow a Book");
            Console.WriteLine("6. Check In a Book");
            Console.WriteLine("7. Save Library");
            Console.WriteLine("8. Exit");
            Console.Write("Choose an option (1-8): ");

            string choice = Console.ReadLine().Trim();

            switch (choice)
            {
                case "1": AddBook(); break;
                case "2": RemoveBook(); break;
                case "3": DisplayBooks(); break;
                case "4": SearchBook(); break;
                case "5": BorrowBook(); break;
                case "6": CheckInBook(); break;
                case "7": SaveToFile(); break;
                case "8":
                    Console.WriteLine(Yellow + "Exiting program..." + Reset);
                    return;
                default:
                    Console.WriteLine(Red + "Invalid option. Choose 1–8." + Reset);
                    break;
            }
        }
    }

    // ---------------------------------------------------------
    // ADD A BOOK
    // ---------------------------------------------------------
    static void AddBook()
    {
        int index = FindEmptySlot();

        if (index == -1)
        {
            Console.WriteLine(Red + "Library is full. Cannot add more books." + Reset);
            return;
        }

        Console.Write("Enter the title of the book to add: ");
        string newBook = Console.ReadLine();

        books[index] = newBook;
        checkedOut[index] = false;

        Console.WriteLine(Green + $"'{newBook}' added to the library." + Reset);
    }

    // ---------------------------------------------------------
    // REMOVE A BOOK
    // ---------------------------------------------------------
    static void RemoveBook()
    {
        Console.Write("Enter the title of the book to remove: ");
        string title = Console.ReadLine();

        int index = FindBook(title);

        if (index == -1)
        {
            Console.WriteLine(Red + "Book not found." + Reset);
            return;
        }

        books[index] = null;
        checkedOut[index] = false;

        Console.WriteLine(Yellow + $"'{title}' removed from the library." + Reset);
    }

    // ---------------------------------------------------------
    // DISPLAY BOOKS
    // ---------------------------------------------------------
    static void DisplayBooks()
    {
        Console.WriteLine(Cyan + "\n=== Books in Library ===" + Reset);

        bool any = false;

        for (int i = 0; i < books.Length; i++)
        {
            if (!string.IsNullOrEmpty(books[i]))
            {
                any = true;
                string status = checkedOut[i] ? Red + " (Checked Out)" + Reset : Green + " (Available)" + Reset;
                Console.WriteLine($"{i + 1}. {books[i]} {status}");
            }
        }

        if (!any)
        {
            Console.WriteLine(Yellow + "[No books in the library]" + Reset);
        }
    }

    // ---------------------------------------------------------
    // SEARCH FOR A BOOK
    // ---------------------------------------------------------
    static void SearchBook()
    {
        Console.Write("Enter the title to search for: ");
        string title = Console.ReadLine();

        int index = FindBook(title);

        if (index == -1)
        {
            Console.WriteLine(Red + $"'{title}' is NOT in the library." + Reset);
        }
        else
        {
            Console.WriteLine(Green + $"'{title}' is available." + Reset);
        }
    }

    // ---------------------------------------------------------
    // BORROW A BOOK
    // ---------------------------------------------------------
    static void BorrowBook()
    {
        if (borrowedCount >= borrowLimit)
        {
            Console.WriteLine(Red + "Borrow limit reached (3 books)." + Reset);
            return;
        }

        Console.Write("Enter the title to borrow: ");
        string title = Console.ReadLine();

        int index = FindBook(title);

        if (index == -1)
        {
            Console.WriteLine(Red + "Book not found." + Reset);
            return;
        }

        if (checkedOut[index])
        {
            Console.WriteLine(Red + "Book is already checked out." + Reset);
            return;
        }

        checkedOut[index] = true;
        borrowedCount++;

        Console.WriteLine(Green + $"You borrowed '{title}'. Borrowed count: {borrowedCount}" + Reset);
    }

    // ---------------------------------------------------------
    // CHECK IN A BOOK
    // ---------------------------------------------------------
    static void CheckInBook()
    {
        Console.Write("Enter the title to check in: ");
        string title = Console.ReadLine();

        int index = FindBook(title);

        if (index == -1)
        {
            Console.WriteLine(Red + "Book not found." + Reset);
            return;
        }

        if (!checkedOut[index])
        {
            Console.WriteLine(Yellow + "This book is not checked out." + Reset);
            return;
        }

        checkedOut[index] = false;
        borrowedCount--;

        Console.WriteLine(Green + $"'{title}' checked in. Borrowed count: {borrowedCount}" + Reset);
    }

    // ---------------------------------------------------------
    // SAVE TO FILE
    // ---------------------------------------------------------
    static void SaveToFile()
    {
        using (StreamWriter writer = new StreamWriter(filePath))
        {
            for (int i = 0; i < books.Length; i++)
            {
                if (!string.IsNullOrEmpty(books[i]))
                {
                    writer.WriteLine($"{books[i]}|{checkedOut[i]}");
                }
            }
        }

        Console.WriteLine(Green + "Library saved to file." + Reset);
    }

    // ---------------------------------------------------------
    // LOAD FROM FILE
    // ---------------------------------------------------------
    static void LoadFromFile()
    {
        if (!File.Exists(filePath)) return;

        string[] lines = File.ReadAllLines(filePath);

        int index = 0;

        foreach (string line in lines)
        {
            if (index >= books.Length) break;

            string[] parts = line.Split('|');
            books[index] = parts[0];
            checkedOut[index] = bool.Parse(parts[1]);
            index++;
        }

        Console.WriteLine(Green + "Library loaded from file." + Reset);
    }

    // ---------------------------------------------------------
    // HELPER METHODS
    // ---------------------------------------------------------
    static int FindEmptySlot()
    {
        for (int i = 0; i < books.Length; i++)
        {
            if (string.IsNullOrEmpty(books[i]))
                return i;
        }
        return -1;
    }

    static int FindBook(string title)
    {
        for (int i = 0; i < books.Length; i++)
        {
            if (books[i] == title)
                return i;
        }
        return -1;
    }
}

