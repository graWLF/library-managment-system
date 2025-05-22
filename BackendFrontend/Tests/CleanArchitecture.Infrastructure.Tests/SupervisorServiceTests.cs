using AutoMapper;
using CleanArchitecture.Core.DTOs.Supervisor;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System;

public class SupervisorServiceTests
{
    private readonly Mock<ISupervisorRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly SupervisorService _service;

    public SupervisorServiceTests()
    {
        _repositoryMock = new Mock<ISupervisorRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new SupervisorService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDTOs()
    {
        var entities = new List<Supervisor>
        {
            new Supervisor { Id = 1, supervisorname = "A" },
            new Supervisor { Id = 2, supervisorname = "B" }
        };
        var dtos = new List<SupervisorDTO>
        {
            new SupervisorDTO { Id = 1, SupervisorName = "A" },
            new SupervisorDTO { Id = 2, SupervisorName = "B" }
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
        _mapperMock.Setup(m => m.Map<IEnumerable<SupervisorDTO>>(entities)).Returns(dtos);

        var result = await _service.GetAllAsync();

        Assert.Equal(dtos, result);
    }

    [Fact]
    public async Task GetByIDAsync_ReturnsMappedDTO()
    {
        var entity = new Supervisor { Id = 1, supervisorname = "A" };
        var dto = new SupervisorDTO { Id = 1, SupervisorName = "A" };

        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<SupervisorDTO>(entity)).Returns(dto);

        var result = await _service.GetByIDAsync(1);

        Assert.Equal(dto, result);
    }

    [Fact]
    public async Task CreateAsync_MapsAndAddsEntity()
    {
        var dto = new SupervisorDTO { Id = 1, SupervisorName = "A" };
        var entity = new Supervisor { Id = 1, supervisorname = "A" };

        _mapperMock.Setup(m => m.Map<Supervisor>(dto)).Returns(entity);

        await _service.CreateAsync(dto);

        _repositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ExistingEntity_MapsAndUpdates()
    {
        var dto = new SupervisorDTO { Id = 1, SupervisorName = "A" };
        var entity = new Supervisor { Id = 1, supervisorname = "A" };

        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(entity);

        await _service.UpdateAsync(1, dto);

        _mapperMock.Verify(m => m.Map(dto, entity), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(entity), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingEntity_ThrowsException()
    {
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync((Supervisor)null);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, new SupervisorDTO()));
    }

    [Fact]
    public async Task DeleteAsync_ExistingEntity_DeletesEntity()
    {
        var entity = new Supervisor { Id = 1, supervisorname = "A" };
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(entity);

        await _service.DeleteAsync(1);

        _repositoryMock.Verify(r => r.DeleteAsync(entity), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingEntity_ThrowsException()
    {
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync((Supervisor)null);

        await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(1));
    }
}