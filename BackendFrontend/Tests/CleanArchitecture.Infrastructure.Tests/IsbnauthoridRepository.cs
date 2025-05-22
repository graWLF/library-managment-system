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

public class IsbnauthoridRepositoryTests
{
    private readonly Mock<ApplicationDbContext> _contextMock;
    private readonly Mock<DbSet<Isbnauthorid>> _dbSetMock;
    private readonly IsbnauthoridRepository _repository;

    public IsbnauthoridRepositoryTests()
    {
        _dbSetMock = new Mock<DbSet<Isbnauthorid>>();
        _contextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>(), null, null);
        _contextMock.Setup(c => c.isbnauthorids).Returns(_dbSetMock.Object);
        _repository = new IsbnauthoridRepository(_contextMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntities()
    {
        var data = new List<Isbnauthorid>
        {
            new Isbnauthorid { Id = 1, authorid = 10 },
            new Isbnauthorid { Id = 2, authorid = 20 }
        }.AsQueryable();

        _dbSetMock.As<IQueryable<Isbnauthorid>>().Setup(m => m.Provider).Returns(data.Provider);
        _dbSetMock.As<IQueryable<Isbnauthorid>>().Setup(m => m.Expression).Returns(data.Expression);
        _dbSetMock.As<IQueryable<Isbnauthorid>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _dbSetMock.As<IQueryable<Isbnauthorid>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _dbSetMock.Setup(d => d.ToListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(data.ToList());

        var result = await _repository.GetAllAsync();

        Assert.Equal(2, result.Count());
        Assert.Contains(result, x => x.Id == 1 && x.authorid == 10);
        Assert.Contains(result, x => x.Id == 2 && x.authorid == 20);
    }

    [Fact]
    public async Task GetByCompositeKeyAsync_ReturnsEntity_WhenFound()
    {
        var data = new List<Isbnauthorid>
        {
            new Isbnauthorid { Id = 1, authorid = 10 },
            new Isbnauthorid { Id = 2, authorid = 20 }
        }.AsQueryable();

        _dbSetMock.As<IQueryable<Isbnauthorid>>().Setup(m => m.Provider).Returns(data.Provider);
        _dbSetMock.As<IQueryable<Isbnauthorid>>().Setup(m => m.Expression).Returns(data.Expression);
        _dbSetMock.As<IQueryable<Isbnauthorid>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _dbSetMock.As<IQueryable<Isbnauthorid>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _dbSetMock.Setup(d => d.FirstOrDefaultAsync(
            It.IsAny<System.Linq.Expressions.Expression<System.Func<Isbnauthorid, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((System.Linq.Expressions.Expression<System.Func<Isbnauthorid, bool>> predicate, CancellationToken token) =>
                data.FirstOrDefault(predicate.Compile()));

        var result = await _repository.GetByCompositeKeyAsync(1, 10);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
        Assert.Equal(10, result.authorid);
    }

    [Fact]
    public async Task GetByCompositeKeyAsync_ReturnsNull_WhenNotFound()
    {
        var data = new List<Isbnauthorid>
        {
            new Isbnauthorid { Id = 1, authorid = 10 }
        }.AsQueryable();

        _dbSetMock.As<IQueryable<Isbnauthorid>>().Setup(m => m.Provider).Returns(data.Provider);
        _dbSetMock.As<IQueryable<Isbnauthorid>>().Setup(m => m.Expression).Returns(data.Expression);
        _dbSetMock.As<IQueryable<Isbnauthorid>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _dbSetMock.As<IQueryable<Isbnauthorid>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _dbSetMock.Setup(d => d.FirstOrDefaultAsync(
            It.IsAny<System.Linq.Expressions.Expression<System.Func<Isbnauthorid, bool>>>(),
            It.IsAny<CancellationToken>()))
            .ReturnsAsync((System.Linq.Expressions.Expression<System.Func<Isbnauthorid, bool>> predicate, CancellationToken token) =>
                data.FirstOrDefault(predicate.Compile()));

        var result = await _repository.GetByCompositeKeyAsync(99, 99);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsEntityAndSaves()
    {
        var entity = new Isbnauthorid { Id = 1, authorid = 10 };

        _dbSetMock.Setup(d => d.Add(entity));
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _repository.AddAsync(entity);

        _dbSetMock.Verify(d => d.Add(entity), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesEntityAndSaves()
    {
        var entity = new Isbnauthorid { Id = 1, authorid = 10 };

        _dbSetMock.Setup(d => d.Update(entity));
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _repository.UpdateAsync(entity);

        _dbSetMock.Verify(d => d.Update(entity), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_RemovesEntityAndSaves()
    {
        var entity = new Isbnauthorid { Id = 1, authorid = 10 };

        _dbSetMock.Setup(d => d.Remove(entity));
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _repository.DeleteAsync(entity);

        _dbSetMock.Verify(d => d.Remove(entity), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}