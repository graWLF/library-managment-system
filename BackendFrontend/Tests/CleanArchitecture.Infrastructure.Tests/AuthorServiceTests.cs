using Xunit;
using Moq;
using AutoMapper;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.DTOs.Author;
using CleanArchitecture.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class AuthorServiceTests
{
    private readonly Mock<IAuthorRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AuthorService _service;

    public AuthorServiceTests()
    {
        _repoMock = new Mock<IAuthorRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new AuthorService(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var authors = new List<Author> { new Author { Id = 1, author = "Test" } };
        var dtos = new List<AuthorDTO> { new AuthorDTO { Id = 1, author = "Test" } };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(authors);
        _mapperMock.Setup(m => m.Map<IEnumerable<AuthorDTO>>(authors)).Returns(dtos);

        var result = await _service.GetAllAsync();

        Assert.Equal(dtos, result);
    }

    [Fact]
    public async Task GetByIDAsync_ReturnsMappedDto()
    {
        var author = new Author { Id = 2, author = "Author2" };
        var dto = new AuthorDTO { Id = 2, author = "Author2" };
        _repoMock.Setup(r => r.GetByIDAsync(2)).ReturnsAsync(author);
        _mapperMock.Setup(m => m.Map<AuthorDTO>(author)).Returns(dto);

        var result = await _service.GetByIDAsync(2);

        Assert.Equal(dto, result);
    }

    [Fact]
    public async Task CreateAsync_MapsAndAddsAuthor()
    {
        var dto = new AuthorDTO { Id = 3, author = "New Author" };
        var author = new Author { Id = 3, author = "New Author" };
        _mapperMock.Setup(m => m.Map<Author>(dto)).Returns(author);

        await _service.CreateAsync(dto);

        _repoMock.Verify(r => r.AddAsync(author), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsIfAuthorNotFound()
    {
        _repoMock.Setup(r => r.GetByIDAsync(4)).ReturnsAsync((Author)null);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(4, new AuthorDTO()));
    }

    [Fact]
    public async Task UpdateAsync_MapsAndUpdatesAuthor()
    {
        var existing = new Author { Id = 5, author = "Old" };
        var dto = new AuthorDTO { Id = 5, author = "Updated" };
        _repoMock.Setup(r => r.GetByIDAsync(5)).ReturnsAsync(existing);

        await _service.UpdateAsync(5, dto);

        _mapperMock.Verify(m => m.Map(dto, existing), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsIfAuthorNotFound()
    {
        _repoMock.Setup(r => r.GetByIDAsync(6)).ReturnsAsync((Author)null);

        await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(6));
    }

    [Fact]
    public async Task DeleteAsync_DeletesAuthor()
    {
        var author = new Author { Id = 7, author = "ToDelete" };
        _repoMock.Setup(r => r.GetByIDAsync(7)).ReturnsAsync(author);

        await _service.DeleteAsync(7);

        _repoMock.Verify(r => r.DeleteAsync(author), Times.Once);
    }
}