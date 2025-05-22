using AutoMapper;
using CleanArchitecture.Core.DTOs.Publisher;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System;

public class PublisherServiceTests
{
    private readonly Mock<IPublisherRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly PublisherService _service;

    public PublisherServiceTests()
    {
        _repositoryMock = new Mock<IPublisherRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new PublisherService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDTOs()
    {
        var entities = new List<Publisher>
        {
            new Publisher { Id = 1, publisher = "A" },
            new Publisher { Id = 2, publisher = "B" }
        };
        var dtos = new List<PublisherDTO>
        {
            new PublisherDTO { Id = 1, publisher = "A" },
            new PublisherDTO { Id = 2, publisher = "B" }
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
        _mapperMock.Setup(m => m.Map<IEnumerable<PublisherDTO>>(entities)).Returns(dtos);

        var result = await _service.GetAllAsync();

        Assert.Equal(dtos, result);
    }

    [Fact]
    public async Task GetByIDAsync_ReturnsMappedDTO()
    {
        var entity = new Publisher { Id = 1, publisher = "A" };
        var dto = new PublisherDTO { Id = 1, publisher = "A" };

        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<PublisherDTO>(entity)).Returns(dto);

        var result = await _service.GetByIDAsync(1);

        Assert.Equal(dto, result);
    }

    [Fact]
    public async Task CreateAsync_MapsAndAddsEntity()
    {
        var dto = new PublisherDTO { Id = 1, publisher = "A" };
        var entity = new Publisher { Id = 1, publisher = "A" };

        _mapperMock.Setup(m => m.Map<Publisher>(dto)).Returns(entity);

        await _service.CreateAsync(dto);

        _repositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ExistingEntity_MapsAndUpdates()
    {
        var dto = new PublisherDTO { Id = 1, publisher = "A" };
        var entity = new Publisher { Id = 1, publisher = "A" };

        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(entity);

        await _service.UpdateAsync(1, dto);

        _mapperMock.Verify(m => m.Map(dto, entity), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(entity), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingEntity_ThrowsException()
    {
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync((Publisher)null);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, new PublisherDTO()));
    }

    [Fact]
    public async Task DeleteAsync_ExistingEntity_DeletesEntity()
    {
        var entity = new Publisher { Id = 1, publisher = "A" };
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(entity);

        await _service.DeleteAsync(1);

        _repositoryMock.Verify(r => r.DeleteAsync(entity), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingEntity_ThrowsException()
    {
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync((Publisher)null);

        await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(1));
    }
}