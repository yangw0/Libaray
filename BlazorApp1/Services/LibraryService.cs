using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using WAD.Models;
using WAD.Components;

namespace WAD.Services

{

    public class LibraryService : ILibraryService
    {
        public LibraryService(string booksPath, string usersPath)
        {
            this.booksPath = booksPath;
            this.usersPath = usersPath;
            ReadBooks();
            ReadUsers();
        }


        // Preserve your original data structures
        private List<Book> books = new List<Book>();
        private List<User> users = new List<User>();
        private Dictionary<User, List<Book>> borrowedBooks = new Dictionary<User, List<Book>>();

        // File paths (moved to wwwroot for Blazor)
        private readonly string booksPath = Path.Combine("Data", "Books.csv");

        private string usersPath = Path.Combine("Data", "Users.csv");

        public LibraryService()
        {
            // Initialize with your original CSV loading
            ReadBooks();
            ReadUsers();
        }

        // 1:1 copy of your original ReadBooks method
        private void ReadBooks()
        {
            try
            {
                if (File.Exists(booksPath))
                {
                    books = File.ReadLines(booksPath)
                        .Select(line => line.Split(','))
                        .Where(fields => fields.Length >= 4)
                        .Select(fields => new Book
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Title = fields[1].Trim(),
                            Author = fields[2].Trim(),
                            ISBN = fields[3].Trim()
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading books: {ex.Message}");
            }
        }

        // 1:1 copy of your original ReadUsers method
        private void ReadUsers()
        {
            try
            {
                if (File.Exists(usersPath))
                {
                    users = File.ReadLines(usersPath)
                        .Select(line => line.Split(','))
                        .Where(fields => fields.Length >= 3)
                        .Select(fields => new User
                        {
                            Id = int.Parse(fields[0].Trim()),
                            Name = fields[1].Trim(),
                            Email = fields[2].Trim()
                        }).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error reading users: {ex.Message}");
            }
        }

        // Preserved Book Operations
        public List<Book> GetAllBooks() => books;

        public Book? GetBookById(int id) => books.FirstOrDefault(b => b.Id == id);

        public void AddBook(Book book)
        {
            // Your original ID generation logic
            int id = books.Any() ? books.Max(b => b.Id) + 1 : 1;
            book.Id = id;
            books.Add(book);
            SaveBooksToCsv();
        }

        public void UpdateBook(Book book)
        {
            var existing = books.FirstOrDefault(b => b.Id == book.Id);
            if (existing != null)
            {
                existing.Title = book.Title;
                existing.Author = book.Author;
                existing.ISBN = book.ISBN;
                SaveBooksToCsv();
            }
        }

        public void DeleteBook(int id)
        {
            var book = books.FirstOrDefault(b => b.Id == id);
            if (book != null)
            {
                books.Remove(book);
                SaveBooksToCsv();
            }
        }

        // Preserved User Operations
        public List<User> GetAllUsers() => users;

        public User? GetUserById(int id) => users.FirstOrDefault(u => u.Id == id);

        public void AddUser(User user)
        {
            // Your original ID generation logic
            int id = users.Any() ? users.Max(u => u.Id) + 1 : 1;
            user.Id = id;
            users.Add(user);
            SaveUsersToCsv();
        }

        public void UpdateUser(User user)
        {
            var existing = users.FirstOrDefault(u => u.Id == user.Id);
            if (existing != null)
            {
                existing.Name = user.Name;
                existing.Email = user.Email;
                SaveUsersToCsv();
            }
        }

        public void DeleteUser(int id)
        {
            var user = users.FirstOrDefault(u => u.Id == id);
            if (user != null)
            {
                users.Remove(user);
                SaveUsersToCsv();
            }
        }

        // Preserved Transaction Logic
        public Dictionary<User, List<Book>> GetBorrowedBooks() => borrowedBooks;

        public void BorrowBook(int userId, int bookId)
        {
            var user = users.FirstOrDefault(u => u.Id == userId);
            var book = books.FirstOrDefault(b => b.Id == bookId);

            if (user != null && book != null)
            {
                if (!borrowedBooks.ContainsKey(user))
                {
                    borrowedBooks[user] = new List<Book>();
                }
                borrowedBooks[user].Add(book);
                books.Remove(book);
            }
        }

        public void ReturnBook(int userId, int bookId)
        {
            var user = users.FirstOrDefault(u => u.Id == userId);
            var book = borrowedBooks[user]?.FirstOrDefault(b => b.Id == bookId);

            if (user != null && book != null)
            {
                borrowedBooks[user].Remove(book);
                books.Add(book);
            }
        }

        // CSV Persistence (matches your original file handling approach)
        public void SaveBooksToCsv()
        {
            try
            {
                var lines = books.Select(b => $"{b.Id},{b.Title},{b.Author},{b.ISBN}");
                File.WriteAllLines(booksPath, new[] { "Id,Title,Author,ISBN" }.Concat(lines));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving books: {ex.Message}");
            }
        }

        public void SaveUsersToCsv()
        {
            try
            {
                var lines = users.Select(u => $"{u.Id},{u.Name},{u.Email}");
                File.WriteAllLines(usersPath, new[] { "Id,Name,Email" }.Concat(lines));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving users: {ex.Message}");
            }
        }
    }
}