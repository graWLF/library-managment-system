using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.WebApi.Controllers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.DTOs.Borrower;

namespace CleanArchitecture.WebApi.Tests.Controllers
{
    public class BorrowerControllerTests
    {
        private readonly Mock<IBorrowerService> _mockService;
        private readonly BorrowerController _controller;

        public BorrowerControllerTests()
        {
            _mockService = new Mock<IBorrowerService>();
            _controller = new BorrowerController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfBorrowers()
        {
            // Arrange
            var borrowers = new List<BorrowerDTO>
            {
                new BorrowerDTO { Id = 1, borrowerName = "John", borrowerPhone = "123" }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(borrowers);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(borrowers, okResult.Value);
        }

        [Fact]
        public async Task GetByID_ReturnsOkResult_WithBorrower()
        {
            // Arrange
            var borrower = new BorrowerDTO { Id = 1, borrowerName = "John", borrowerPhone = "123" };
            _mockService.Setup(s => s.GetByIDAsync(1)).ReturnsAsync(borrower);

            // Act
            var result = await _controller.GetByID(1);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(borrower, okResult.Value);
        }

        [Fact]
        public async Task Create_CallsService_AndReturnsOk()
        {
            // Arrange
            var dto = new BorrowerDTO { Id = 1, borrowerName = "John", borrowerPhone = "123" };
            _mockService.Setup(s => s.CreateAsync(dto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Create(dto);

            // Assert
            _mockService.Verify(s => s.CreateAsync(dto), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Update_CallsService_AndReturnsOk()
        {
            // Arrange
            var dto = new BorrowerDTO { Id = 1, borrowerName = "John", borrowerPhone = "123" };
            _mockService.Setup(s => s.UpdateAsync(1, dto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, dto);

            // Assert
            _mockService.Verify(s => s.UpdateAsync(1, dto), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_CallsService_AndReturnsOk()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1);

            // Assert
            _mockService.Verify(s => s.DeleteAsync(1), Times.Once);
            Assert.IsType<OkResult>(result);
        }
    }
}