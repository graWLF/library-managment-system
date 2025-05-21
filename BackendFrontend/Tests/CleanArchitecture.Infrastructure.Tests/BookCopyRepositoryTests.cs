using Xunit;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Contexts;
using CleanArchitecture.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;

public class BookCopyRepositoryTest
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;

        var dateTimeService = new Mock<CleanArchitecture.Core.Interfaces.IDateTimeService>();
        var authenticatedUserService = new Mock<CleanArchitecture.Core.Interfaces.IAuthenticatedUserService>();
        return new ApplicationDbContext(options, dateTimeService.Object, authenticatedUserService.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBookCopies()
    {
        using var context = CreateContext();
        var bookCopies = new List<BookCopy>
        {
            new BookCopy { Id = 1, isbn = 100, location = "A" },
            new BookCopy { Id = 2, isbn = 200, location = "B" }
        };
        context.BookCopies.AddRange(bookCopies);
        context.SaveChanges();

        var repo = new BookCopyRepository(context);

        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count());
        Assert.Contains(result, b => b.location == "A");
        Assert.Contains(result, b => b.location == "B");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsBookCopy_WhenExists()
    {
        using var context = CreateContext();
        var bookCopy = new BookCopy { Id = 10, isbn = 300, location = "C" };
        context.BookCopies.Add(bookCopy);
        context.SaveChanges();

        var repo = new BookCopyRepository(context);

        var result = await repo.GetByIdAsync(10);

        Assert.NotNull(result);
        Assert.Equal("C", result.location);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        using var context = CreateContext();
        var repo = new BookCopyRepository(context);

        var result = await repo.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsBookCopyToDatabase()
    {
        using var context = CreateContext();
        var repo = new BookCopyRepository(context);
        var bookCopy = new BookCopy { Id = 20, isbn = 400, location = "D" };

        await repo.AddAsync(bookCopy);

        var dbBookCopy = context.BookCopies.Find(20);
        Assert.NotNull(dbBookCopy);
        Assert.Equal("D", dbBookCopy.location);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesBookCopyInDatabase()
    {
        using var context = CreateContext();
        var bookCopy = new BookCopy { Id = 30, isbn = 500, location = "E" };
        context.BookCopies.Add(bookCopy);
        context.SaveChanges();

        var repo = new BookCopyRepository(context);
        bookCopy.location = "Updated";

        await repo.UpdateAsync(bookCopy);

        var dbBookCopy = context.BookCopies.Find(30);
        Assert.Equal("Updated", dbBookCopy.location);
    }

    [Fact]
    public async Task DeleteAsync_RemovesBookCopyFromDatabase()
    {
        using var context = CreateContext();
        var bookCopy = new BookCopy { Id = 40, isbn = 600, location = "F" };
        context.BookCopies.Add(bookCopy);
        context.SaveChanges();

        var repo = new BookCopyRepository(context);

        await repo.DeleteAsync(bookCopy);

        var dbBookCopy = context.BookCopies.Find(40);
        Assert.Null(dbBookCopy);
    }
}