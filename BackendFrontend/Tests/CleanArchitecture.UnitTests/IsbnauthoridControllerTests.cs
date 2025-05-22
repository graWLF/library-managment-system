using CleanArchitecture.Core.DTOs.Isbnauthorid;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.WebApi.Tests.Controllers
{
    public class IsbnauthoridControllerTests
    {
        private readonly Mock<IIsbnauthoridService> _serviceMock;
        private readonly IsbnauthoridController _controller;

        public IsbnauthoridControllerTests()
        {
            _serviceMock = new Mock<IIsbnauthoridService>();
            _controller = new IsbnauthoridController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithResult()
        {
            var dtos = new List<IsbnauthoridDTO>
        {
            new IsbnauthoridDTO { Id = 1, AuthorId = 10 },
            new IsbnauthoridDTO { Id = 2, AuthorId = 20 }
        };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(dtos);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtos, okResult.Value);
        }

        [Fact]
        public async Task GetByCompositeKey_ReturnsOkWithResult_WhenFound()
        {
            var dto = new IsbnauthoridDTO { Id = 1, AuthorId = 10 };
            _serviceMock.Setup(s => s.GetByCompositeKeyAsync(1, 10)).ReturnsAsync(dto);

            var result = await _controller.GetByCompositeKey(1, 10);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dto, okResult.Value);
        }

        [Fact]
        public async Task GetByCompositeKey_ReturnsNotFound_WhenNotFound()
        {
            _serviceMock.Setup(s => s.GetByCompositeKeyAsync(1, 10)).ReturnsAsync((IsbnauthoridDTO)null);

            var result = await _controller.GetByCompositeKey(1, 10);

            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_CallsServiceAndReturnsOk()
        {
            var dto = new IsbnauthoridDTO { Id = 1, AuthorId = 10 };
            _serviceMock.Setup(s => s.CreateAsync(dto)).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.CreateAsync(dto), Times.Once);
        }

        [Fact]
        public async Task Update_CallsServiceAndReturnsOk()
        {
            var dto = new IsbnauthoridDTO { Id = 1, AuthorId = 10 };
            _serviceMock.Setup(s => s.UpdateAsync(1, 10, dto)).Returns(Task.CompletedTask);

            var result = await _controller.Update(1, 10, dto);

            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.UpdateAsync(1, 10, dto), Times.Once);
        }

        [Fact]
        public async Task Delete_CallsServiceAndReturnsOk()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1, 10)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1, 10);

            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.DeleteAsync(1, 10), Times.Once);
        }
    }
}