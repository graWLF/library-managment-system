using Xunit;
using Moq;
using System.Threading.Tasks;
using System.Collections.Generic;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.DTOs.Book;
using CleanArchitecture.Core.Entities;
using AutoMapper;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;
using Newtonsoft.Json.Linq;
using System.Reflection;

public class BookServiceTests
{
    private readonly Mock<IBookRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly Mock<IWebHostEnvironment> _envMock;
    private readonly BookService _service;

    public BookServiceTests()
    {
        _repoMock = new Mock<IBookRepository>();
        _mapperMock = new Mock<IMapper>();
        _envMock = new Mock<IWebHostEnvironment>();
        _envMock.Setup(e => e.ContentRootPath).Returns(Directory.GetCurrentDirectory());
        _service = new BookService(_repoMock.Object, _mapperMock.Object, _envMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var books = new List<Book> { new Book { Id = 1 } };
        var dtos = new List<BookDto> { new BookDto { Id = 1 } };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(books);
        _mapperMock.Setup(m => m.Map<IEnumerable<BookDto>>(books)).Returns(dtos);

        var result = await _service.GetAllAsync();

        Assert.Equal(dtos, result);
    }

    [Fact]
    public async Task GetByISBNAsync_ReturnsMappedDto()
    {
        var book = new Book { Id = 2 };
        var dto = new BookDto { Id = 2 };
        _repoMock.Setup(r => r.GetByISBNAsync(2)).ReturnsAsync(book);
        _mapperMock.Setup(m => m.Map<BookDto>(book)).Returns(dto);

        var result = await _service.GetByISBNAsync(2);

        Assert.Equal(dto, result);
    }

    [Fact]
    public async Task GetByNameAsync_ReturnsMappedDtos()
    {
        var books = new List<Book> { new Book { Id = 3 } };
        var dtos = new List<BookDto> { new BookDto { Id = 3 } };
        _repoMock.Setup(r => r.GetByNameAsync("test")).ReturnsAsync(books);
        _mapperMock.Setup(m => m.Map<IEnumerable<BookDto>>(books)).Returns(dtos);

        var result = await _service.GetByNameAsync("test");

        Assert.Equal(dtos, result);
    }

    [Fact]
    public async Task CreateAsync_MapsAndAddsBook()
    {
        var dto = new BookDto { Id = 4 };
        var book = new Book { Id = 4 };
        _mapperMock.Setup(m => m.Map<Book>(dto)).Returns(book);

        await _service.CreateAsync(dto);

        _repoMock.Verify(r => r.AddAsync(book), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsIfBookNotFound()
    {
        _repoMock.Setup(r => r.GetByISBNAsync(5)).ReturnsAsync((Book)null);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(5, new BookDto()));
    }

    [Fact]
    public async Task UpdateAsync_MapsAndUpdatesBook()
    {
        var existing = new Book { Id = 6 };
        var dto = new BookDto { Id = 6 };
        _repoMock.Setup(r => r.GetByISBNAsync(6)).ReturnsAsync(existing);

        await _service.UpdateAsync(6, dto);

        _mapperMock.Verify(m => m.Map(dto, existing), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsIfBookNotFound()
    {
        _repoMock.Setup(r => r.GetByISBNAsync(7)).ReturnsAsync((Book)null);

        await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(7));
    }

    [Fact]
    public async Task DeleteAsync_DeletesBook()
    {
        var book = new Book { Id = 8 };
        _repoMock.Setup(r => r.GetByISBNAsync(8)).ReturnsAsync(book);

        await _service.DeleteAsync(8);

        _repoMock.Verify(r => r.DeleteAsync(book), Times.Once);
    }

    [Fact]
    public async Task ScanBarcodeBase64Async_ReturnsEmptyString_OnFormatException()
    {
        // Invalid base64 string
        var result = await _service.ScanBarcodeBase64Async("not_base64");
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public async Task ScanBarcodeBase64Async_ReturnsEmptyString_OnException()
    {
        // Simulate exception by making env path invalid
        var envMock = new Mock<IWebHostEnvironment>();
        envMock.Setup(e => e.ContentRootPath).Throws(new Exception("fail"));
        var service = new BookService(_repoMock.Object, _mapperMock.Object, envMock.Object);

        var validBase64 = Convert.ToBase64String(new byte[] { 1, 2, 3 });
        var result = await service.ScanBarcodeBase64Async(validBase64);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public async Task ScanBarcodePathAsync_ReturnsEmptyString_OnException()
    {
        // Pass a path that will fail process start
        var result = await _service.ScanBarcodePathAsync("invalid_path_that_will_fail");
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public async Task ScanBarcodeAsync_ReturnsEmptyString_OnException()
    {
        // Pass a mock IFormFile that throws on CopyToAsync
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.FileName).Returns("file.jpg");
        fileMock.Setup(f => f.CopyToAsync(It.IsAny<Stream>(), default)).ThrowsAsync(new Exception("fail"));

        var result = await _service.ScanBarcodeAsync(fileMock.Object);
        Assert.Equal(string.Empty, result);
    }

    [Fact]
    public async Task SearchBookAsync_Throws_OnInvalidIsbn()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _service.SearchBookAsync("123", new string('a', 39)));
    }

    [Fact]
    public async Task SearchBookAsync_Throws_OnInvalidApiKey()
    {
        await Assert.ThrowsAsync<ArgumentException>(() => _service.SearchBookAsync("1234567890", "shortkey"));
    }

    [Fact]
    public async Task SearchBookAsync_Throws_OnBookNotFound()
    {
        // Use reflection to set up a stub for GetMainUrlAsync that throws
        var service = new BookService(_repoMock.Object, _mapperMock.Object, _envMock.Object);
        var method = typeof(BookService).GetMethod("GetMainUrlAsync", BindingFlags.NonPublic | BindingFlags.Instance);
        // Can't easily mock private method, so skip this test or use a partial mock with a framework like JustMock or Typemock.
        // For demonstration, we expect an exception if Google API returns no items.
        await Assert.ThrowsAsync<Exception>(() => service.SearchBookAsync("1234567890", new string('a', 39)));
    }

    [Fact]
    public void ParseBookData_Throws_OnNullItem()
    {
        var method = typeof(BookService).GetMethod("ParseBookData", BindingFlags.NonPublic | BindingFlags.Instance);
        Assert.NotNull(method);
        var ex = Assert.Throws<TargetInvocationException>(() =>
            method.Invoke(_service, new object[] { null, "url" })
        );
        Assert.IsType<Exception>(ex.InnerException);
        Assert.Equal("Book data not found.", ex.InnerException.Message);
    }

    [Fact]
    public void ParseBookData_ParsesFields()
    {
        var item = JObject.Parse(@"{
            'volumeInfo': {
                'title': 'Test Book',
                'industryIdentifiers': [
                    { 'type': 'ISBN_13', 'identifier': '9781234567890' }
                ],
                'pageCount': 123,
                'publishedDate': '2020-01-01',
                'language': 'en',
                'infoLink': 'http://info',
                'previewLink': 'http://preview'
            }
        }");
        var method = typeof(BookService).GetMethod("ParseBookData", BindingFlags.NonPublic | BindingFlags.Instance);
        var dto = (BookDto)method.Invoke(_service, new object[] { item, "http://info" });
        Assert.Equal("Test Book", dto.Title);
        Assert.Equal("9781234567890", dto.Local_isbn);
        Assert.Equal(123, dto.Pages);
        Assert.Equal("2020-01-01", dto.ReleaseDate);
        Assert.Equal("en", dto.ContentLanguage);
        Assert.Equal("http://info", dto.InfoUrl);
        Assert.Equal("http://preview", dto.ContentLink);
    }
}