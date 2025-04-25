using WAD.Models;
using System.Collections.Generic;
using WAD;
using WAD.Components;

namespace WAD.Services
{
    public interface ILibraryService
    {
        // Books
        List<Book> GetAllBooks();
        Book? GetBookById(int id);
        void AddBook(Book book);
        void UpdateBook(Book book);
        void DeleteBook(int id);

        // Users
        List<User> GetAllUsers();
        User? GetUserById(int id);
        void AddUser(User user);
        void UpdateUser(User user);
        void DeleteUser(int id);

        // Transactions
        Dictionary<User, List<Book>> GetBorrowedBooks();
        void BorrowBook(int userId, int bookId);
        void ReturnBook(int userId, int bookId);

        // CSV
        void SaveBooksToCsv();
        void SaveUsersToCsv();
    }
}