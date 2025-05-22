using AutoMapper;
using CleanArchitecture.Core.DTOs.Registration;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System;

public class RegistrationServiceTests
{
    private readonly Mock<IRegistrationRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly RegistrationService _service;

    public RegistrationServiceTests()
    {
        _repositoryMock = new Mock<IRegistrationRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new RegistrationService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDTOs()
    {
        var entities = new List<Registration>
        {
            new Registration { Id = 1, Username = "user1" },
            new Registration { Id = 2, Username = "user2" }
        };
        var dtos = new List<RegistrationDTO>
        {
            new RegistrationDTO { Id = 1, Username = "user1" },
            new RegistrationDTO { Id = 2, Username = "user2" }
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
        _mapperMock.Setup(m => m.Map<IEnumerable<RegistrationDTO>>(entities)).Returns(dtos);

        var result = await _service.GetAllAsync();

        Assert.Equal(dtos, result);
    }

    [Fact]
    public async Task GetByIDAsync_ReturnsMappedDTO()
    {
        var entity = new Registration { Id = 1, Username = "user1" };
        var dto = new RegistrationDTO { Id = 1, Username = "user1" };

        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<RegistrationDTO>(entity)).Returns(dto);

        var result = await _service.GetByIDAsync(1);

        Assert.Equal(dto, result);
    }

    [Fact]
    public async Task CreateAsync_MapsAndAddsEntity()
    {
        var dto = new RegistrationDTO { Id = 1, Username = "user1" };
        var entity = new Registration { Id = 1, Username = "user1" };

        _mapperMock.Setup(m => m.Map<Registration>(dto)).Returns(entity);

        await _service.CreateAsync(dto);

        _repositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ExistingEntity_MapsAndUpdates()
    {
        var dto = new RegistrationDTO { Id = 1, Username = "user1" };
        var entity = new Registration { Id = 1, Username = "user1" };

        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(entity);

        await _service.UpdateAsync(1, dto);

        _mapperMock.Verify(m => m.Map(dto, entity), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(entity), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingEntity_ThrowsException()
    {
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync((Registration)null);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, new RegistrationDTO()));
    }

    [Fact]
    public async Task DeleteAsync_ExistingEntity_DeletesEntity()
    {
        var entity = new Registration { Id = 1, Username = "user1" };
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(entity);

        await _service.DeleteAsync(1);

        _repositoryMock.Verify(r => r.DeleteAsync(entity), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingEntity_ThrowsException()
    {
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync((Registration)null);

        await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(1));
    }

    [Fact]
    public void CheckSupervisor_ReturnsRepositoryResult()
    {
        _repositoryMock.Setup(r => r.CheckSupervisor("user", "pass")).Returns(true);

        var result = _service.CheckSupervisor("user", "pass");

        Assert.True(result);
        _repositoryMock.Verify(r => r.CheckSupervisor("user", "pass"), Times.Once);
    }

    [Fact]
    public void Login_ReturnsRepositoryResult()
    {
        _repositoryMock.Setup(r => r.Login("user", "pass")).Returns(true);

        var result = _service.Login("user", "pass");

        Assert.True(result);
        _repositoryMock.Verify(r => r.Login("user", "pass"), Times.Once);
    }
}