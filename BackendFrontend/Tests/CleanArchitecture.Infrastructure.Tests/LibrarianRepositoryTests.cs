using CleanArchitecture.Core.Entities;
using CleanArchitecture.Infrastructure.Contexts;
using CleanArchitecture.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

public class LibrarianRepositoryTests
{
    private readonly Mock<ApplicationDbContext> _contextMock;
    private readonly Mock<DbSet<Librarian>> _dbSetMock;
    private readonly LibrarianRepository _repository;

    public LibrarianRepositoryTests()
    {
        _dbSetMock = new Mock<DbSet<Librarian>>();
        _contextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>(), null, null);
        _contextMock.Setup(c => c.librarians).Returns(_dbSetMock.Object);
        _repository = new LibrarianRepository(_contextMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntities()
    {
        var data = new List<Librarian>
        {
            new Librarian { Id = 1, librarianname = "A" },
            new Librarian { Id = 2, librarianname = "B" }
        }.AsQueryable();

        _dbSetMock.As<IQueryable<Librarian>>().Setup(m => m.Provider).Returns(data.Provider);
        _dbSetMock.As<IQueryable<Librarian>>().Setup(m => m.Expression).Returns(data.Expression);
        _dbSetMock.As<IQueryable<Librarian>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _dbSetMock.As<IQueryable<Librarian>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _dbSetMock.Setup(d => d.ToListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(data.ToList());

        var result = await _repository.GetAllAsync();

        Assert.Equal(2, result.Count());
        Assert.Contains(result, x => x.Id == 1 && x.librarianname == "A");
        Assert.Contains(result, x => x.Id == 2 && x.librarianname == "B");
    }

    [Fact]
    public async Task GetByIDAsync_ReturnsEntity_WhenFound()
    {
        var librarian = new Librarian { Id = 1, librarianname = "A" };
        _dbSetMock.Setup(d => d.FindAsync(1))
            .ReturnsAsync(librarian);

        var result = await _repository.GetByIDAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIDAsync_ReturnsNull_WhenNotFound()
    {
        _dbSetMock.Setup(d => d.FindAsync(99))
            .ReturnsAsync((Librarian)null);

        var result = await _repository.GetByIDAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsEntityAndSaves()
    {
        var entity = new Librarian { Id = 1, librarianname = "A" };

        _dbSetMock.Setup(d => d.Add(entity));
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _repository.AddAsync(entity);

        _dbSetMock.Verify(d => d.Add(entity), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesEntityAndSaves()
    {
        var entity = new Librarian { Id = 1, librarianname = "A" };

        _dbSetMock.Setup(d => d.Update(entity));
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _repository.UpdateAsync(entity);

        _dbSetMock.Verify(d => d.Update(entity), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_RemovesEntityAndSaves()
    {
        var entity = new Librarian { Id = 1, librarianname = "A" };

        _dbSetMock.Setup(d => d.Remove(entity));
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _repository.DeleteAsync(entity);

        _dbSetMock.Verify(d => d.Remove(entity), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}