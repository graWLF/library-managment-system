using Xunit;
using Moq;
using WebApi.Controllers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.DTOs.Author;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class AuthorControllerTests
{
    private readonly Mock<IAuthorService> _serviceMock;
    private readonly AuthorController _controller;

    public AuthorControllerTests()
    {
        _serviceMock = new Mock<IAuthorService>();
        _controller = new AuthorController(_serviceMock.Object);
    }

    [Fact]
    public async Task GetAll_ReturnsOkWithResult()
    {
        var dtos = new List<AuthorDTO> { new AuthorDTO { Id = 1, author = "Test" } };
        _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(dtos);

        var result = await _controller.GetAll();

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dtos, ok.Value);
    }

    [Fact]
    public async Task GetByID_ReturnsOkWithResult()
    {
        var dto = new AuthorDTO { Id = 2, author = "Author2" };
        _serviceMock.Setup(s => s.GetByIDAsync(2)).ReturnsAsync(dto);

        var result = await _controller.GetByID(2);

        var ok = Assert.IsType<OkObjectResult>(result);
        Assert.Equal(dto, ok.Value);
    }

    [Fact]
    public async Task Create_ReturnsOk()
    {
        var dto = new AuthorDTO { Id = 3, author = "New Author" };
        _serviceMock.Setup(s => s.CreateAsync(dto)).Returns(Task.CompletedTask);

        var result = await _controller.Create(dto);

        Assert.IsType<OkResult>(result);
        _serviceMock.Verify(s => s.CreateAsync(dto), Times.Once);
    }

    [Fact]
    public async Task Update_ReturnsOk()
    {
        var dto = new AuthorDTO { Id = 4, author = "Updated" };
        _serviceMock.Setup(s => s.UpdateAsync(4, dto)).Returns(Task.CompletedTask);

        var result = await _controller.Update(4, dto);

        Assert.IsType<OkResult>(result);
        _serviceMock.Verify(s => s.UpdateAsync(4, dto), Times.Once);
    }

    [Fact]
    public async Task Delete_ReturnsOk()
    {
        _serviceMock.Setup(s => s.DeleteAsync(5)).Returns(Task.CompletedTask);

        var result = await _controller.Delete(5);

        Assert.IsType<OkResult>(result);
        _serviceMock.Verify(s => s.DeleteAsync(5), Times.Once);
    }
}