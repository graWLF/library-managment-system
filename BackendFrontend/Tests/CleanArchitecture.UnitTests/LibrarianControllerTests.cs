using CleanArchitecture.Core.DTOs.Librarian;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.WebApi.Tests.Controllers
{
    public class LibrarianControllerTests
    {
        private readonly Mock<ILibrarianService> _serviceMock;
        private readonly LibrarianController _controller;

        public LibrarianControllerTests()
        {
            _serviceMock = new Mock<ILibrarianService>();
            _controller = new LibrarianController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithResult()
        {
            var dtos = new List<LibrarianDTO>
        {
            new LibrarianDTO { Id = 1, LibrarianName = "A" },
            new LibrarianDTO { Id = 2, LibrarianName = "B" }
        };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(dtos);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtos, okResult.Value);
        }

        [Fact]
        public async Task GetByID_ReturnsOkWithResult()
        {
            var dto = new LibrarianDTO { Id = 1, LibrarianName = "A" };
            _serviceMock.Setup(s => s.GetByIDAsync(1)).ReturnsAsync(dto);

            var result = await _controller.GetByID(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dto, okResult.Value);
        }

        [Fact]
        public async Task Create_CallsServiceAndReturnsOk()
        {
            var dto = new LibrarianDTO { Id = 1, LibrarianName = "A" };
            _serviceMock.Setup(s => s.CreateAsync(dto)).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.CreateAsync(dto), Times.Once);
        }

        [Fact]
        public async Task Update_CallsServiceAndReturnsOk()
        {
            var dto = new LibrarianDTO { Id = 1, LibrarianName = "A" };
            _serviceMock.Setup(s => s.UpdateAsync(1, dto)).Returns(Task.CompletedTask);

            var result = await _controller.Update(1, dto);

            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.UpdateAsync(1, dto), Times.Once);
        }

        [Fact]
        public async Task Delete_CallsServiceAndReturnsOk()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.DeleteAsync(1), Times.Once);
        }
    }
}