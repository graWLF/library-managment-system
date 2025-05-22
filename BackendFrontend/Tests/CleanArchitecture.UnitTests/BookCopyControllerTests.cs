using Xunit;
using Moq;
using CleanArchitecture.WebApi.Controllers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.DTOs.BookCopy;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Tests.Controllers
{
    public class BookCopyControllerTests
    {
        private readonly Mock<IBookCopyService> _serviceMock;
        private readonly BookCopyController _controller;

        public BookCopyControllerTests()
        {
            _serviceMock = new Mock<IBookCopyService>();
            _controller = new BookCopyController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithResult()
        {
            var dtos = new List<BookCopyDTO> { new BookCopyDTO { Id = 1, Isbn = 123, Location = "A" } };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(dtos);

            var result = await _controller.GetAll();

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtos, ok.Value);
        }

        [Fact]
        public async Task GetByID_ReturnsOkWithResult()
        {
            var dto = new BookCopyDTO { Id = 2, Isbn = 456, Location = "B" };
            _serviceMock.Setup(s => s.GetByIdAsync(2)).ReturnsAsync(dto);

            var result = await _controller.GetByID(2);

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dto, ok.Value);
        }

        [Fact]
        public async Task Create_ReturnsOk()
        {
            var dto = new BookCopyDTO { Id = 3, Isbn = 789, Location = "C" };
            _serviceMock.Setup(s => s.CreateAsync(dto)).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.CreateAsync(dto), Times.Once);
        }

        [Fact]
        public async Task Update_ReturnsOk()
        {
            var dto = new BookCopyDTO { Id = 4, Isbn = 111, Location = "D" };
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
}