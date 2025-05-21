using Xunit;
using CleanArchitecture.Infrastructure.Repositories;
using CleanArchitecture.Infrastructure.Contexts;
using CleanArchitecture.Core.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Moq;

public class BorrowerRepositoryTest
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
    public async Task GetAllAsync_ReturnsAllBorrowers()
    {
        using var context = CreateContext();
        var borrowers = new List<Borrower>
        {
            new Borrower { Id = 1, borrowername = "Ali", borrowerphone = "111" },
            new Borrower { Id = 2, borrowername = "Veli", borrowerphone = "222" }
        };
        context.Borrowers.AddRange(borrowers);
        context.SaveChanges();

        var repo = new BorrowerRepository(context);

        var result = await repo.GetAllAsync();

        Assert.Equal(2, result.Count());
        Assert.Contains(result, b => b.borrowername == "Ali");
        Assert.Contains(result, b => b.borrowername == "Veli");
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsBorrower_WhenExists()
    {
        using var context = CreateContext();
        var borrower = new Borrower { Id = 10, borrowername = "Ayşe", borrowerphone = "333" };
        context.Borrowers.Add(borrower);
        context.SaveChanges();

        var repo = new BorrowerRepository(context);

        var result = await repo.GetByIdAsync(10);

        Assert.NotNull(result);
        Assert.Equal("Ayşe", result.borrowername);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsNull_WhenNotExists()
    {
        using var context = CreateContext();
        var repo = new BorrowerRepository(context);

        var result = await repo.GetByIdAsync(999);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsBorrowerToDatabase()
    {
        using var context = CreateContext();
        var repo = new BorrowerRepository(context);
        var borrower = new Borrower { Id = 20, borrowername = "Fatma", borrowerphone = "444" };

        await repo.AddAsync(borrower);

        var dbBorrower = context.Borrowers.Find(20L);
        Assert.NotNull(dbBorrower);
        Assert.Equal("Fatma", dbBorrower.borrowername);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesBorrowerInDatabase()
    {
        using var context = CreateContext();
        var borrower = new Borrower { Id = 30, borrowername = "Eski", borrowerphone = "555" };
        context.Borrowers.Add(borrower);
        context.SaveChanges();

        var repo = new BorrowerRepository(context);
        borrower.borrowername = "Yeni";

        await repo.UpdateAsync(borrower);

        var dbBorrower = context.Borrowers.Find(30L);
        Assert.Equal("Yeni", dbBorrower.borrowername);
    }

    [Fact]
    public async Task DeleteAsync_RemovesBorrowerFromDatabase()
    {
        using var context = CreateContext();
        var borrower = new Borrower { Id = 40, borrowername = "Silinecek", borrowerphone = "666" };
        context.Borrowers.Add(borrower);
        context.SaveChanges();

        var repo = new BorrowerRepository(context);

        await repo.DeleteAsync(borrower);

        var dbBorrower = context.Borrowers.Find(40L);
        Assert.Null(dbBorrower);
    }
}