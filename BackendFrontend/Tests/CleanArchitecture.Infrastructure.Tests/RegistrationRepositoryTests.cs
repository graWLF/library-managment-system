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

public class RegistrationRepositoryTests
{
    private readonly Mock<ApplicationDbContext> _contextMock;
    private readonly Mock<DbSet<Registration>> _dbSetMock;
    private readonly RegistrationRepository _repository;

    public RegistrationRepositoryTests()
    {
        _dbSetMock = new Mock<DbSet<Registration>>();
        _contextMock = new Mock<ApplicationDbContext>(new DbContextOptions<ApplicationDbContext>(), null, null);
        _contextMock.Setup(c => c.registrations).Returns(_dbSetMock.Object);
        _repository = new RegistrationRepository(_contextMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsAllEntities()
    {
        var data = new List<Registration>
        {
            new Registration { Id = 1, Username = "user1" },
            new Registration { Id = 2, Username = "user2" }
        }.AsQueryable();

        _dbSetMock.As<IQueryable<Registration>>().Setup(m => m.Provider).Returns(data.Provider);
        _dbSetMock.As<IQueryable<Registration>>().Setup(m => m.Expression).Returns(data.Expression);
        _dbSetMock.As<IQueryable<Registration>>().Setup(m => m.ElementType).Returns(data.ElementType);
        _dbSetMock.As<IQueryable<Registration>>().Setup(m => m.GetEnumerator()).Returns(data.GetEnumerator());

        _dbSetMock.Setup(d => d.ToListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(data.ToList());

        var result = await _repository.GetAllAsync();

        Assert.Equal(2, result.Count());
        Assert.Contains(result, x => x.Username == "user1");
        Assert.Contains(result, x => x.Username == "user2");
    }

    [Fact]
    public async Task GetByIDAsync_ReturnsEntity_WhenFound()
    {
        var registration = new Registration { Id = 1, Username = "user1" };
        _dbSetMock.Setup(d => d.FindAsync(1))
            .ReturnsAsync(registration);

        var result = await _repository.GetByIDAsync(1);

        Assert.NotNull(result);
        Assert.Equal(1, result.Id);
    }

    [Fact]
    public async Task GetByIDAsync_ReturnsNull_WhenNotFound()
    {
        _dbSetMock.Setup(d => d.FindAsync(99))
            .ReturnsAsync((Registration)null);

        var result = await _repository.GetByIDAsync(99);

        Assert.Null(result);
    }

    [Fact]
    public async Task AddAsync_AddsEntityAndSaves()
    {
        var entity = new Registration { Id = 1, Username = "user1" };

        _dbSetMock.Setup(d => d.Add(entity));
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _repository.AddAsync(entity);

        _dbSetMock.Verify(d => d.Add(entity), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_UpdatesEntityAndSaves()
    {
        var entity = new Registration { Id = 1, Username = "user1" };

        _dbSetMock.Setup(d => d.Update(entity));
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _repository.UpdateAsync(entity);

        _dbSetMock.Verify(d => d.Update(entity), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_RemovesEntityAndSaves()
    {
        var entity = new Registration { Id = 1, Username = "user1" };

        _dbSetMock.Setup(d => d.Remove(entity));
        _contextMock.Setup(c => c.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);

        await _repository.DeleteAsync(entity);

        _dbSetMock.Verify(d => d.Remove(entity), Times.Once);
        _contextMock.Verify(c => c.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public void CheckSupervisor_ReturnsTrue_WhenSupervisorCredentialsMatch()
    {
        var registrations = new List<Registration>
        {
            new Registration { Username = "super", Password = "pass", AuthLevel = 2, Email = "super@email.com" }
        };
        // Setup GetAllAsync to return our registrations synchronously
        _dbSetMock.Setup(d => d.ToListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(registrations);

        // Use a partial mock to override GetAllAsync().Result
        var repo = new Mock<RegistrationRepository>(_contextMock.Object) { CallBase = true };
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(registrations);

        // Username match
        Assert.True(repo.Object.CheckSupervisor("super", "pass"));
        // Email match
        Assert.True(repo.Object.CheckSupervisor("super@email.com", "pass"));
        // Wrong password
        Assert.False(repo.Object.CheckSupervisor("super", "wrong"));
        // Not supervisor
        Assert.False(repo.Object.CheckSupervisor("super", "pass1"));
    }

    [Fact]
    public void Login_ReturnsTrue_WhenCredentialsMatch()
    {
        var registrations = new List<Registration>
        {
            new Registration { Username = "user", Password = "pass", AuthLevel = 1, Email = "user@email.com" }
        };
        _dbSetMock.Setup(d => d.ToListAsync(It.IsAny<CancellationToken>()))
            .ReturnsAsync(registrations);

        var repo = new Mock<RegistrationRepository>(_contextMock.Object) { CallBase = true };
        repo.Setup(r => r.GetAllAsync()).ReturnsAsync(registrations);

        // Username match
        Assert.True(repo.Object.Login("user", "pass"));
        // Email match
        Assert.True(repo.Object.Login("user@email.com", "pass"));
        // Wrong password
        Assert.False(repo.Object.Login("user", "wrong"));
        // Wrong username
        Assert.False(repo.Object.Login("nouser", "pass"));
    }
}