using Xunit;
using Moq;
using WebAPI.Controllers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.DTOs.Book;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class BookControllerTests
{
    private readonly Mock<IBookService> _serviceMock;
    private readonly BookController _controller;

    public BookControllerTests()
    {
        _serviceMock = new Mock<IBookService>();
        _controller = new BookController(_serviceMock.Object);
    }

    [Fact]
    public async Task ScanBarcode_Base64ImageRequest_ReturnsBadRequest_WhenImageIsNullOrEmpty()
    {
        var result = await _controller.ScanBarcode(new BookController.Base64ImageRequest { ImageBase64 = "" });
        Assert.IsType<BadRequestObjectResult>(result);
    }

    [Fact]
    public async Task ScanBarcode_Base64ImageRequest_ReturnsOk_WhenValid()
    {
        _serviceMock.Setup(s => s.ScanBarcodePathAsync(It.IsAny<string>())).ReturnsAsync("barcode");
        var result = await _controller.ScanBarcode(new BookController.Base64ImageRequest { ImageBase64 = "base64" });
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("barcode", ((dynamic)ok.Value).barcode);
    }

    [Fact]
    public async Task ScanBarcode_Path_ReturnsBadRequest_WhenPathIsNullOrEmpty()
    {
        var result1 = await _controller.ScanBarcode((string)null);
        var result2 = await _controller.ScanBarcode("");
        Assert.IsType<BadRequestObjectResult>(result1);
        Assert.IsType<BadRequestObjectResult>(result2);
    }

    [Fact]
    public async Task ScanBarcode_Path_ReturnsNotFound_WhenNoBarcode()
    {
        _serviceMock.Setup(s => s.ScanBarcodePathAsync(It.IsAny<string>())).ReturnsAsync("");
        var result = await _controller.ScanBarcode("validpath");
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task ScanBarcode_Path_ReturnsOk_WhenBarcodeFound()
    {
        _serviceMock.Setup(s => s.ScanBarcodePathAsync(It.IsAny<string>())).ReturnsAsync("barcode");
        var result = await _controller.ScanBarcode("validpath");
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("barcode", ((dynamic)ok.Value).barcode);
    }

    [Fact]
    public async Task ScanBarcode_Path_ReturnsServerError_OnException()
    {
        _serviceMock.Setup(s => s.ScanBarcodePathAsync(It.IsAny<string>())).ThrowsAsync(new Exception("fail"));
        var result = await _controller.ScanBarcode("validpath");
        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, obj.StatusCode);
    }

    [Fact]
    public async Task ScanBarcode_IFormFile_ReturnsBadRequest_WhenNullOrEmpty()
    {
        var result1 = await _controller.ScanBarcode((IFormFile)null);

        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(0);
        var result2 = await _controller.ScanBarcode(fileMock.Object);

        Assert.IsType<BadRequestObjectResult>(result1);
        Assert.IsType<BadRequestObjectResult>(result2);
    }

    [Fact]
    public async Task ScanBarcode_IFormFile_ReturnsNotFound_WhenNoBarcode()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(1);
        _serviceMock.Setup(s => s.ScanBarcodeAsync(It.IsAny<IFormFile>())).ReturnsAsync("");
        var result = await _controller.ScanBarcode(fileMock.Object);
        Assert.IsType<NotFoundObjectResult>(result);
    }

    [Fact]
    public async Task ScanBarcode_IFormFile_ReturnsOk_WhenBarcodeFound()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(1);
        _serviceMock.Setup(s => s.ScanBarcodeAsync(It.IsAny<IFormFile>())).ReturnsAsync("barcode");
        var result = await _controller.ScanBarcode(fileMock.Object);
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal("barcode", ((dynamic)ok.Value).barcode);
    }

    [Fact]
    public async Task ScanBarcode_IFormFile_ReturnsServerError_OnException()
    {
        var fileMock = new Mock<IFormFile>();
        fileMock.Setup(f => f.Length).Returns(1);
        _serviceMock.Setup(s => s.ScanBarcodeAsync(It.IsAny<IFormFile>())).ThrowsAsync(new Exception("fail"));
        var result = await _controller.ScanBarcode(fileMock.Object);
        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, obj.StatusCode);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithResult()
    {
        var dtos = new List<BookDto> { new BookDto { Id = 1 } };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(dtos);
        var result = await _controller.GetAll();
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dtos, ok.Value);
    }

    [Fact]
    public async Task SearchBookWeb_ReturnsOkWithBook()
    {
        var dto = new BookDto { Id = 2 };
        _serviceMock.Setup(s => s.SearchBookAsync("isbn", "apiKey")).ReturnsAsync(dto);
        var result = await _controller.SearchBookWeb("isbn", "apiKey");
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto, ok.Value);
    }

    [Fact]
    public async Task SearchBookWeb_ReturnsServerError_OnException()
    {
        _serviceMock.Setup(s => s.SearchBookAsync(It.IsAny<string>(), It.IsAny<string>())).ThrowsAsync(new Exception("fail"));
        var result = await _controller.SearchBookWeb("isbn", "apiKey");
        var obj = Assert.IsType<ObjectResult>(result);
        Assert.Equal(500, obj.StatusCode);
    }

    [Fact]
    public async Task GetByISBN_ReturnsOkWithResult()
    {
        var dto = new BookDto { Id = 3 };
        _serviceMock.Setup(s => s.GetByISBNAsync(3)).ReturnsAsync(dto);
        var result = await _controller.GetByISBN(3);
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto, ok.Value);
    }

    [Fact]
    public async Task GetByName_ReturnsOkWithResult()
    {
        var dtos = new List<BookDto> { new BookDto { Id = 4 } };
        _serviceMock.Setup(s => s.GetByNameAsync("name")).ReturnsAsync(dtos);
        var result = await _controller.GetByName("name");
        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dtos, ok.Value);
    }

    [Fact]
    public async Task Create_ReturnsOk()
    {
        var dto = new BookDto();
        _serviceMock.Setup(s => s.CreateAsync(dto)).Returns(Task.CompletedTask);
        var result = await _controller.Create(dto);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Update_ReturnsOk()
    {
        var dto = new BookDto();
        _serviceMock.Setup(s => s.UpdateAsync(5, dto)).Returns(Task.CompletedTask);
        var result = await _controller.Update(5, dto);
        Assert.IsType<OkResult>(result);
    }

    [Fact]
    public async Task Delete_ReturnsOk()
    {
        _serviceMock.Setup(s => s.DeleteAsync(6)).Returns(Task.CompletedTask);
        var result = await _controller.Delete(6);
        Assert.IsType<OkResult>(result);
    }
}