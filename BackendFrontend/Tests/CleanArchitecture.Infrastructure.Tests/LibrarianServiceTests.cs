using AutoMapper;
using CleanArchitecture.Core.DTOs.Librarian;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Services;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System;

public class LibrarianServiceTests
{
    private readonly Mock<ILibrarianRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly LibrarianService _service;

    public LibrarianServiceTests()
    {
        _repositoryMock = new Mock<ILibrarianRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new LibrarianService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDTOs()
    {
        var entities = new List<Librarian>
        {
            new Librarian { Id = 1, librarianname = "A" },
            new Librarian { Id = 2, librarianname = "B" }
        };
        var dtos = new List<LibrarianDTO>
        {
            new LibrarianDTO { Id = 1, LibrarianName = "A" },
            new LibrarianDTO { Id = 2, LibrarianName = "B" }
        };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(entities);
        _mapperMock.Setup(m => m.Map<IEnumerable<LibrarianDTO>>(entities)).Returns(dtos);

        var result = await _service.GetAllAsync();

        Assert.Equal(dtos, result);
    }

    [Fact]
    public async Task GetByIDAsync_ReturnsMappedDTO()
    {
        var entity = new Librarian { Id = 1, librarianname = "A" };
        var dto = new LibrarianDTO { Id = 1, LibrarianName = "A" };

        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(entity);
        _mapperMock.Setup(m => m.Map<LibrarianDTO>(entity)).Returns(dto);

        var result = await _service.GetByIDAsync(1);

        Assert.Equal(dto, result);
    }

    [Fact]
    public async Task CreateAsync_MapsAndAddsEntity()
    {
        var dto = new LibrarianDTO { Id = 1, LibrarianName = "A" };
        var entity = new Librarian { Id = 1, librarianname = "A" };

        _mapperMock.Setup(m => m.Map<Librarian>(dto)).Returns(entity);

        await _service.CreateAsync(dto);

        _repositoryMock.Verify(r => r.AddAsync(entity), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ExistingEntity_MapsAndUpdates()
    {
        var dto = new LibrarianDTO { Id = 1, LibrarianName = "A" };
        var entity = new Librarian { Id = 1, librarianname = "A" };

        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(entity);

        await _service.UpdateAsync(1, dto);

        _mapperMock.Verify(m => m.Map(dto, entity), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(entity), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingEntity_ThrowsException()
    {
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync((Librarian)null);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, new LibrarianDTO()));
    }

    [Fact]
    public async Task DeleteAsync_ExistingEntity_DeletesEntity()
    {
        var entity = new Librarian { Id = 1, librarianname = "A" };
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(entity);

        await _service.DeleteAsync(1);

        _repositoryMock.Verify(r => r.DeleteAsync(entity), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingEntity_ThrowsException()
    {
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync((Librarian)null);

        await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(1));
    }
}