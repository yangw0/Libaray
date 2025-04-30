using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using WAD.Models;
using WAD.Services;

namespace TestProjectWAD
{

    [TestClass]
    public class LibraryServiceUnitTests
    {
        [TestMethod]
        public void AddBook_Should_Work_Correctly()
        {
            var booksPath = Path.Combine("TestData", "Books_UnitTest.csv");
            var usersPath = Path.Combine("TestData", "Users_UnitTest.csv");
            Directory.CreateDirectory("TestData");
            File.WriteAllText(booksPath, "Id,Title,Author,ISBN");
            File.WriteAllText(usersPath, "Id,Name,Email");

            var service = new LibraryService(booksPath, usersPath);
            var book = new Book { Title = "Test", Author = "Author", ISBN = "000" };

            service.AddBook(book);
            var books = service.GetAllBooks();

            Assert.AreEqual(1, books.Count);
            Assert.AreEqual("Test", books[0].Title);
        }

        [TestMethod]
        public void UpdateBook_Should_Work_Correctly()
        {
            var booksPath = Path.Combine("TestData", "Books_UnitTest.csv");
            var usersPath = Path.Combine("TestData", "Users_UnitTest.csv");
            Directory.CreateDirectory("TestData");
            File.WriteAllText(booksPath, "Id,Title,Author,ISBN");
            File.WriteAllText(usersPath, "Id,Name,Email");

            var service = new LibraryService(booksPath, usersPath);
            var book = new Book { Title = "Old", Author = "A", ISBN = "123" };
            service.AddBook(book);
            var added = service.GetAllBooks().First();
            added.Title = "New";

            service.UpdateBook(added);
            var updated = service.GetBookById(added.Id);

            Assert.AreEqual("New", updated.Title);
        }

        [TestMethod]
        public void DeleteBook_Should_Work_Correctly()
        {
            var booksPath = Path.Combine("TestData", "Books_UnitTest.csv");
            var usersPath = Path.Combine("TestData", "Users_UnitTest.csv");
            Directory.CreateDirectory("TestData");
            File.WriteAllText(booksPath, "Id,Title,Author,ISBN");
            File.WriteAllText(usersPath, "Id,Name,Email");

            var service = new LibraryService(booksPath, usersPath);
            var book = new Book { Title = "Temp", Author = "A", ISBN = "321" };
            service.AddBook(book);
            var id = service.GetAllBooks().First().Id;

            service.DeleteBook(id);
            var books = service.GetAllBooks();

            Assert.AreEqual(0, books.Count);
        }

        [TestMethod]
        public void AddUser_Should_Work_Correctly()
        {
            var booksPath = Path.Combine("TestData", "Books_UnitTest.csv");
            var usersPath = Path.Combine("TestData", "Users_UnitTest.csv");
            Directory.CreateDirectory("TestData");
            File.WriteAllText(booksPath, "Id,Title,Author,ISBN");
            File.WriteAllText(usersPath, "Id,Name,Email");

            var service = new LibraryService(booksPath, usersPath);
            var user = new User { Name = "Jane", Email = "jane@example.com" };

            service.AddUser(user);
            var users = service.GetAllUsers();

            Assert.AreEqual(1, users.Count);
            Assert.AreEqual("Jane", users[0].Name);
        }

        [TestMethod]
        public void UpdateUser_Should_Work_Correctly()
        {
            var booksPath = Path.Combine("TestData", "Books_UnitTest.csv");
            var usersPath = Path.Combine("TestData", "Users_UnitTest.csv");
            Directory.CreateDirectory("TestData");
            File.WriteAllText(booksPath, "Id,Title,Author,ISBN");
            File.WriteAllText(usersPath, "Id,Name,Email");

            var service = new LibraryService(booksPath, usersPath);
            var user = new User { Name = "Old", Email = "old@mail.com" };
            service.AddUser(user);
            var existing = service.GetAllUsers().First();
            existing.Name = "New";

            service.UpdateUser(existing);
            var updated = service.GetUserById(existing.Id);

            Assert.AreEqual("New", updated.Name);
        }

        [TestMethod]
        public void DeleteUser_Should_Work_Correctly()
        {
            var booksPath = Path.Combine("TestData", "Books_UnitTest.csv");
            var usersPath = Path.Combine("TestData", "Users_UnitTest.csv");
            Directory.CreateDirectory("TestData");
            File.WriteAllText(booksPath, "Id,Title,Author,ISBN");
            File.WriteAllText(usersPath, "Id,Name,Email");

            var service = new LibraryService(booksPath, usersPath);
            var user = new User { Name = "Delete", Email = "del@example.com" };
            service.AddUser(user);
            var id = service.GetAllUsers().First().Id;

            service.DeleteUser(id);
            var users = service.GetAllUsers();

            Assert.AreEqual(0, users.Count);
        }

        [TestMethod]
        public void BorrowBook_Should_Work_Correctly()
        {
            var booksPath = Path.Combine("TestData", "Books_UnitTest.csv");
            var usersPath = Path.Combine("TestData", "Users_UnitTest.csv");
            Directory.CreateDirectory("TestData");
            File.WriteAllText(booksPath, "Id,Title,Author,ISBN");
            File.WriteAllText(usersPath, "Id,Name,Email");

            var service = new LibraryService(booksPath, usersPath);
            var user = new User { Name = "U", Email = "u@mail.com" };
            var book = new Book { Title = "Borrow", Author = "A", ISBN = "111" };
            service.AddUser(user);
            service.AddBook(book);
            var userId = service.GetAllUsers().First().Id;
            var bookId = service.GetAllBooks().First().Id;

            service.BorrowBook(userId, bookId);
            var borrowed = service.GetBorrowedBooks();

            Assert.IsTrue(borrowed.Values.SelectMany(b => b).Any(b => b.Id == bookId));
        }

        [TestMethod]
        public void ReturnBook_Should_Work_Correctly()
        {
            var booksPath = Path.Combine("TestData", "Books_UnitTest.csv");
            var usersPath = Path.Combine("TestData", "Users_UnitTest.csv");
            Directory.CreateDirectory("TestData");
            File.WriteAllText(booksPath, "Id,Title,Author,ISBN");
            File.WriteAllText(usersPath, "Id,Name,Email");

            var service = new LibraryService(booksPath, usersPath);
            var user = new User { Name = "U", Email = "u@mail.com" };
            var book = new Book { Title = "Return", Author = "A", ISBN = "222" };
            service.AddUser(user);
            service.AddBook(book);
            var userId = service.GetAllUsers().First().Id;
            var bookId = service.GetAllBooks().First().Id;
            service.BorrowBook(userId, bookId);

            service.ReturnBook(userId, bookId);
            var allBooks = service.GetAllBooks();

            Assert.IsTrue(allBooks.Any(b => b.Id == bookId));
        }

       

        [TestMethod]
        public void ReadUsers_Should_Not_Work_Correctly()
        {
            var booksPath = Path.Combine("TestData", "Books_UnitTest.csv");
            var usersPath = Path.Combine("TestData", "Users_UnitTest.csv");
            Directory.CreateDirectory("TestData");

            File.WriteAllText(booksPath, "Id,Title,Author,ISBN");
            File.WriteAllLines(usersPath, new[]
            {
                "Id,Name,Email",
                "1,Alice,alice@mail.com",
                "2,Bob,bob@mail.com"
            });

            var serviceReload = new LibraryService(booksPath, usersPath);
            var users = serviceReload.GetAllUsers();

            Assert.AreEqual(2, users.Count);
            Assert.AreEqual("Alice", users[0].Name);
        }

        [TestMethod]
        public void SaveBooksToCsv_Should_Work_Correctly()
        {
            var booksPath = Path.Combine("TestData", "Books_UnitTest.csv");
            var usersPath = Path.Combine("TestData", "Users_UnitTest.csv");
            Directory.CreateDirectory("TestData");
            File.WriteAllText(booksPath, "Id,Title,Author,ISBN");
            File.WriteAllText(usersPath, "Id,Name,Email");

            var service = new LibraryService(booksPath, usersPath);
            var book = new Book { Title = "CSV Book", Author = "CSV Author", ISBN = "999" };
            service.AddBook(book);

            service.SaveBooksToCsv();
            var lines = File.ReadAllLines(booksPath);

            Assert.IsTrue(lines.Any(line => line.Contains("CSV Book")));
        }

        [TestMethod]
        public void SaveUsersToCsv_Should_Work_Correctly()
        {
            var booksPath = Path.Combine("TestData", "Books_UnitTest.csv");
            var usersPath = Path.Combine("TestData", "Users_UnitTest.csv");
            Directory.CreateDirectory("TestData");
            File.WriteAllText(booksPath, "Id,Title,Author,ISBN");
            File.WriteAllText(usersPath, "Id,Name,Email");

            var service = new LibraryService(booksPath, usersPath);
            var user = new User { Name = "CSV User", Email = "csv@example.com" };
            service.AddUser(user);

            service.SaveUsersToCsv();
            var lines = File.ReadAllLines(usersPath);

            Assert.IsTrue(lines.Any(line => line.Contains("CSV User")));
        }
    }
}
