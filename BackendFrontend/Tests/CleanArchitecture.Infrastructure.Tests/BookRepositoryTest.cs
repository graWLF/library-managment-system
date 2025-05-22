using Xunit;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Contexts;
using CleanArchitecture.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class BookRepositoryTest
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;

        // You may need to mock IDateTimeService and IAuthenticatedUserService if required by your context constructor
        var dateTimeService = new Moq.Mock<CleanArchitecture.Core.Interfaces.IDateTimeService>();
        var authenticatedUserService = new Moq.Mock<CleanArchitecture.Core.Interfaces.IAuthenticatedUserService>();
        return new ApplicationDbContext(options, dateTimeService.Object, authenticatedUserService.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBooks()
    {
        using var context = CreateContext();
        var books = new List<Book>
        {
            new Book { Id = 1, title = "Book1" },
            new Book { Id = 2, title = "Book2" }
        };
        context.Books.AddRange(books);
        context.SaveChanges();

        var repo = new BookRepository(context);

        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count());
        Assert.Contains(result, b => b.title == "Book1");
        Assert.Contains(result, b => b.title == "Book2");
    }

    [Fact]
    public async Task GetByISBNAsync_ReturnsBook_WhenExists()
    {
        using var context = CreateContext();
        var book = new Book { Id = 123, title = "TestBook" };
        context.Books.Add(book);
        context.SaveChanges();

        var repo = new BookRepository(context);

        var result = await repo.GetByISBNAsync(123);

        Assert.NotNull(result);
        Assert.Equal("TestBook", result.title);
    }

    [Fact]
    public async Task GetByISBNAsync_ReturnsNull_WhenNotExists()
    {
        using var context = CreateContext();
        var repo = new BookRepository(context);

        var result = await repo.GetByISBNAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task GetByNameAsync_ReturnsMatchingBooks()
    {
        using var context = CreateContext();
        context.Books.Add(new Book { Id = 1, title = "C# Programming" });
        context.Books.Add(new Book { Id = 2, title = "Java Programming" });
        context.Books.Add(new Book { Id = 3, title = "Python" });
        context.SaveChanges();

        var repo = new BookRepository(context);

        var result = await repo.GetByNameAsync("Programming");

        Assert.Equal(2, result.Count());
        Assert.All(result, b => Assert.Contains("Programming", b.title));
    }

    [Fact]
    public async Task AddAsync_AddsBookToDatabase()
    {
        using var context = CreateContext();
        var repo = new BookRepository(context);
        var book = new Book { Id = 10, title = "New Book" };

        await repo.AddAsync(book);

        var dbBook = context.Books.Find(10L);
        Assert.NotNull(dbBook);
        Assert.Equal("New Book", dbBook.title);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesBookInDatabase()
    {
        using var context = CreateContext();
        var book = new Book { Id = 20, title = "Old Title" };
        context.Books.Add(book);
        context.SaveChanges();

        var repo = new BookRepository(context);
        book.title = "Updated Title";

        await repo.UpdateAsync(book);

        var dbBook = context.Books.Find(20L);
        Assert.Equal("Updated Title", dbBook.title);
    }

    [Fact]
    public async Task DeleteAsync_RemovesBookFromDatabase()
    {
        using var context = CreateContext();
        var book = new Book { Id = 30, title = "To Delete" };
        context.Books.Add(book);
        context.SaveChanges();

        var repo = new BookRepository(context);

        await repo.DeleteAsync(book);

        var dbBook = context.Books.Find(30L);
        Assert.Null(dbBook);
    }
}