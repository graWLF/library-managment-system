using AutoMapper;
using CleanArchitecture.Core.DTOs.Isbnauthorid;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System;

public class IsbnauthoridServiceTests
{
    private readonly Mock<IIsbnauthoridRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly IsbnauthoridService _service;

    public IsbnauthoridServiceTests()
    {
        _repositoryMock = new Mock<IIsbnauthoridRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new IsbnauthoridService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDTOs()
    {
        var entities = new List<Isbnauthorid>
        {
            new Isbnauthorid { Id = 1, authorid = 10 },
            new Isbnauthorid { Id = 2, authorid = 20 }
        };
        var dtos = new List<IsbnauthoridDTO>
        {
            new IsbnauthoridDTO { Id = 1, AuthorId = 10 },
            new IsbnauthoridDTO { Id = 2, AuthorId = 20 }
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
        _mapperMock.Setup(m => m.Map<IEnumerable<IsbnauthoridDTO>>(entities)).Returns(dtos);

        var result = await _service.GetAllAsync();

        Assert.Equal(dtos, result);
    }

    [Fact]
    public async Task GetByCompositeKeyAsync_ReturnsMappedDTO()
    {
        var entity = new Isbnauthorid { Id = 1, authorid = 10 };
        var dto = new IsbnauthoridDTO { Id = 1, AuthorId = 10 };

        _repositoryMock.Setup(r => r.GetByCompositeKeyAsync(1, 10)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<IsbnauthoridDTO>(entity)).Returns(dto);

        var result = await _service.GetByCompositeKeyAsync(1, 10);

        Assert.Equal(dto, result);
    }

    [Fact]
    public async Task CreateAsync_MapsAndAddsEntity()
    {
        var dto = new IsbnauthoridDTO { Id = 1, AuthorId = 10 };
        var entity = new Isbnauthorid { Id = 1, authorid = 10 };

        _mapperMock.Setup(m => m.Map<Isbnauthorid>(dto)).Returns(entity);

        await _service.CreateAsync(dto);

        _repositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ExistingEntity_MapsAndUpdates()
    {
        var dto = new IsbnauthoridDTO { Id = 1, AuthorId = 10 };
        var entity = new Isbnauthorid { Id = 1, authorid = 10 };

        _repositoryMock.Setup(r => r.GetByCompositeKeyAsync(1, 10)).ReturnsAsync(entity);

        await _service.UpdateAsync(1, 10, dto);

        _mapperMock.Verify(m => m.Map(dto, entity), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(entity), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingEntity_ThrowsException()
    {
        _repositoryMock.Setup(r => r.GetByCompositeKeyAsync(1, 10)).ReturnsAsync((Isbnauthorid)null);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, 10, new IsbnauthoridDTO()));
    }

    [Fact]
    public async Task DeleteAsync_ExistingEntity_DeletesEntity()
    {
        var entity = new Isbnauthorid { Id = 1, authorid = 10 };
        _repositoryMock.Setup(r => r.GetByCompositeKeyAsync(1, 10)).ReturnsAsync(entity);

        await _service.DeleteAsync(1, 10);

        _repositoryMock.Verify(r => r.DeleteAsync(entity), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingEntity_ThrowsException()
    {
        _repositoryMock.Setup(r => r.GetByCompositeKeyAsync(1, 10)).ReturnsAsync((Isbnauthorid)null);

        await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(1, 10));
    }
}