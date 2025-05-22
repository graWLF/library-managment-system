using Xunit;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Contexts;
using CleanArchitecture.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;

public class AuthorRepositoryTest
{
    private ApplicationDbContext CreateContext()
    {
        var options = new DbContextOptionsBuilder<ApplicationDbContext>()
            .UseInMemoryDatabase(databaseName: System.Guid.NewGuid().ToString())
            .Options;

        // mock neceserry services
        var dateTimeService = new Mock<CleanArchitecture.Core.Interfaces.IDateTimeService>();
        var authenticatedUserService = new Mock<CleanArchitecture.Core.Interfaces.IAuthenticatedUserService>();
        return new ApplicationDbContext(options, dateTimeService.Object, authenticatedUserService.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllAuthors()
    {
        using var context = CreateContext();
        var authors = new List<Author>
        {
            new Author { Id = 1, author = "Author1" },
            new Author { Id = 2, author = "Author2" }
        };
        context.Authors.AddRange(authors);
        context.SaveChanges();

        var repo = new AuthorRepository(context);

        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count());
        Assert.Contains(result, a => a.author == "Author1");
        Assert.Contains(result, a => a.author == "Author2");
    }

    [Fact]
    public async Task GetByIDAsync_ReturnsAuthor_WhenExists()
    {
        using var context = CreateContext();
        var author = new Author { Id = 10, author = "TestAuthor" };
        context.Authors.Add(author);
        context.SaveChanges();

        var repo = new AuthorRepository(context);

        var result = await repo.GetByIDAsync(10);

        Assert.NotNull(result);
        Assert.Equal("TestAuthor", result.author);
    }

    [Fact]
    public async Task GetByIDAsync_ReturnsNull_WhenNotExists()
    {
        using var context = CreateContext();
        var repo = new AuthorRepository(context);

        var result = await repo.GetByIDAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsAuthorToDatabase()
    {
        using var context = CreateContext();
        var repo = new AuthorRepository(context);
        var author = new Author { Id = 20, author = "New Author" };

        await repo.AddAsync(author);

        var dbAuthor = context.Authors.Find(20);
        Assert.NotNull(dbAuthor);
        Assert.Equal("New Author", dbAuthor.author);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesAuthorInDatabase()
    {
        using var context = CreateContext();
        var author = new Author { Id = 30, author = "Old Name" };
        context.Authors.Add(author);
        context.SaveChanges();

        var repo = new AuthorRepository(context);
        author.author = "Updated Name";

        await repo.UpdateAsync(author);

        var dbAuthor = context.Authors.Find(30);
        Assert.Equal("Updated Name", dbAuthor.author);
    }

    [Fact]
    public async Task DeleteAsync_RemovesAuthorFromDatabase()
    {
        using var context = CreateContext();
        var author = new Author { Id = 40, author = "To Delete" };
        context.Authors.Add(author);
        context.SaveChanges();

        var repo = new AuthorRepository(context);

        await repo.DeleteAsync(author);

        var dbAuthor = context.Authors.Find(40);
        Assert.Null(dbAuthor);
    }
}