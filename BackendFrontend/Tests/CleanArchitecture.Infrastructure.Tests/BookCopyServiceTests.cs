using Xunit;
using Moq;
using AutoMapper;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.DTOs.BookCopy;
using CleanArchitecture.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class BookCopyServiceTests
{
    private readonly Mock<IBookCopyRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly BookCopyService _service;

    public BookCopyServiceTests()
    {
        _repoMock = new Mock<IBookCopyRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new BookCopyService(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var bookCopies = new List<BookCopy> { new BookCopy { Id = 1, isbn = 123, location = "A" } };
        var dtos = new List<BookCopyDTO> { new BookCopyDTO { Id = 1, Isbn = 123, Location = "A" } };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(bookCopies);
        _mapperMock.Setup(m => m.Map<IEnumerable<BookCopyDTO>>(bookCopies)).Returns(dtos);

        var result = await _service.GetAllAsync();

        Assert.Equal(dtos, result);
    }

    [Fact]
    public async Task GetByIdAsync_ReturnsMappedDto()
    {
        var bookCopy = new BookCopy { Id = 2, isbn = 456, location = "B" };
        var dto = new BookCopyDTO { Id = 2, Isbn = 456, Location = "B" };
        _repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(bookCopy);
        _mapperMock.Setup(m => m.Map<BookCopyDTO>(bookCopy)).Returns(dto);

        var result = await _service.GetByIdAsync(2);

        Assert.Equal(dto, result);
    }

    [Fact]
    public async Task CreateAsync_MapsAndAddsBookCopy()
    {
        var dto = new BookCopyDTO { Id = 3, Isbn = 789, Location = "C" };
        var bookCopy = new BookCopy { Id = 3, isbn = 789, location = "C" };
        _mapperMock.Setup(m => m.Map<BookCopy>(dto)).Returns(bookCopy);

        await _service.CreateAsync(dto);

        _repoMock.Verify(r => r.AddAsync(bookCopy), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsIfBookCopyNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(4)).ReturnsAsync((BookCopy)null);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(4, new BookCopyDTO()));
    }

    [Fact]
    public async Task UpdateAsync_MapsAndUpdatesBookCopy()
    {
        var existing = new BookCopy { Id = 5, isbn = 111, location = "D" };
        var dto = new BookCopyDTO { Id = 5, Isbn = 111, Location = "Updated" };
        _repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(existing);

        await _service.UpdateAsync(5, dto);

        _mapperMock.Verify(m => m.Map(dto, existing), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsIfBookCopyNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(6)).ReturnsAsync((BookCopy)null);

        await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(6));
    }

    [Fact]
    public async Task DeleteAsync_DeletesBookCopy()
    {
        var bookCopy = new BookCopy { Id = 7, isbn = 222, location = "E" };
        _repoMock.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(bookCopy);

        await _service.DeleteAsync(7);

        _repoMock.Verify(r => r.DeleteAsync(bookCopy), Times.Once);
    }
}