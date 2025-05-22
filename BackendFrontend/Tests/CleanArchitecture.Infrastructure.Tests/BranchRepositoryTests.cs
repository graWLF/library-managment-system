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

public class BranchRepositoryTests
{
    private readonly Mock<ApplicationDbContext> _contextMock;
    private readonly Mock<DbSet<Branch>> _dbSetMock;
    private readonly BranchRepository _repository;

    public BranchRepositoryTests()
    {
        _dbSetMock = new Mock<DbSet<Branch>>();
        _contextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>(), null, null);
        _contextMock.Setup(c => c.branches).Returns(_dbSetMock.Object);
        _repository = new BranchRepository(_contextMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllBranches()
    {
        var data = new List<Branch>
        {
            new Branch { Id = 1, branchname = "A" },
            new Branch { Id = 2, branchname = "B" }
        }.AsQueryable();

        _dbSetMock.As<IQueryable<Branch>>().Setup(m => m.Provider).Returns(data.Provider);
        _dbSetMock.As<IQueryable<Branch>>().Setup(m => m.Expression).Returns(data.Expression);
        _dbSetMock.As<IQueryable<Branch>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _dbSetMock.As<IQueryable<Branch>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _dbSetMock.Setup(d => d.ToListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(data.ToList());

        var result = await _repository.GetAllAsync();

        Assert.Equal(2, result.Count());
        Assert.Contains(result, b => b.branchname == "A");
        Assert.Contains(result, b => b.branchname == "B");
    }

    [Fact]
    public async Task GetByIDAsync_ReturnsBranch_WhenFound()
    {
        var branch = new Branch { Id = 1, branchname = "A" };
        _dbSetMock.Setup(d => d.FindAsync(1))
            .ReturnsAsync(branch);

        var result = await _repository.GetByIDAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIDAsync_ReturnsNull_WhenNotFound()
    {
        _dbSetMock.Setup(d => d.FindAsync(99))
            .ReturnsAsync((Branch)null);

        var result = await _repository.GetByIDAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsBranchAndSaves()
    {
        var branch = new Branch { Id = 1, branchname = "A" };

        _dbSetMock.Setup(d => d.Add(branch));
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _repository.AddAsync(branch);

        _dbSetMock.Verify(d => d.Add(branch), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesBranchAndSaves()
    {
        var branch = new Branch { Id = 1, branchname = "A" };

        _dbSetMock.Setup(d => d.Update(branch));
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _repository.UpdateAsync(branch);

        _dbSetMock.Verify(d => d.Update(branch), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_RemovesBranchAndSaves()
    {
        var branch = new Branch { Id = 1, branchname = "A" };

        _dbSetMock.Setup(d => d.Remove(branch));
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _repository.DeleteAsync(branch);

        _dbSetMock.Verify(d => d.Remove(branch), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}