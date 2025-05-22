using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using CleanArchitecture.WebApi.Controllers;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.DTOs.Borrowing;

namespace CleanArchitecture.WebApi.Tests.Controllers
{
    public class BorrowingControllerTests
    {
        private readonly Mock<IBorrowingService> _mockService;
        private readonly BorrowingController _controller;

        public BorrowingControllerTests()
        {
            _mockService = new Mock<IBorrowingService>();
            _controller = new BorrowingController(_mockService.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkResult_WithListOfBorrowings()
        {
            // Arrange
            var borrowings = new List<BorrowingDTO>
            {
                new BorrowingDTO { Id = 1, BorrowerId = 2, BranchId = 3, BorrowDate = "2024-01-01", DueDate = "2024-01-10", ReturnStatus = false }
            };
            _mockService.Setup(s => s.GetAllAsync()).ReturnsAsync(borrowings);

            // Act
            var result = await _controller.GetAll();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(borrowings, okResult.Value);
        }

        [Fact]
        public async Task GetByCompositeKey_ReturnsOkResult_WhenFound()
        {
            // Arrange
            var dto = new BorrowingDTO { Id = 1, BorrowerId = 2, BranchId = 3, BorrowDate = "2024-01-01", DueDate = "2024-01-10", ReturnStatus = false };
            _mockService.Setup(s => s.GetByCompositeKeyAsync(1, 2, "2024-01-01", "2024-01-10")).ReturnsAsync(dto);

            // Act
            var result = await _controller.GetByCompositeKey(1, 2, "2024-01-01", "2024-01-10");

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dto, okResult.Value);
        }

        [Fact]
        public async Task GetByCompositeKey_ReturnsNotFound_WhenNull()
        {
            // Arrange
            _mockService.Setup(s => s.GetByCompositeKeyAsync(1, 2, "2024-01-01", "2024-01-10")).ReturnsAsync((BorrowingDTO)null);

            // Act
            var result = await _controller.GetByCompositeKey(1, 2, "2024-01-01", "2024-01-10");

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public async Task Create_CallsService_AndReturnsOk()
        {
            // Arrange
            var dto = new BorrowingDTO { Id = 1, BorrowerId = 2, BranchId = 3, BorrowDate = "2024-01-01", DueDate = "2024-01-10", ReturnStatus = false };
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
            var dto = new BorrowingDTO { Id = 1, BorrowerId = 2, BranchId = 3, BorrowDate = "2024-01-01", DueDate = "2024-01-10", ReturnStatus = false };
            _mockService.Setup(s => s.UpdateAsync(1, 2, "2024-01-01", "2024-01-10", dto)).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Update(1, 2, "2024-01-01", "2024-01-10", dto);

            // Assert
            _mockService.Verify(s => s.UpdateAsync(1, 2, "2024-01-01", "2024-01-10", dto), Times.Once);
            Assert.IsType<OkResult>(result);
        }

        [Fact]
        public async Task Delete_CallsService_AndReturnsOk()
        {
            // Arrange
            _mockService.Setup(s => s.DeleteAsync(1, 2, "2024-01-01", "2024-01-10")).Returns(Task.CompletedTask);

            // Act
            var result = await _controller.Delete(1, 2, "2024-01-01", "2024-01-10");

            // Assert
            _mockService.Verify(s => s.DeleteAsync(1, 2, "2024-01-01", "2024-01-10"), Times.Once);
            Assert.IsType<OkResult>(result);
        }
    }
}