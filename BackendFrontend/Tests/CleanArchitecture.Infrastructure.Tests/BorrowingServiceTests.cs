using Xunit;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.DTOs.Borrowing;
using CleanArchitecture.Core.Entities;
using System;


    public class BorrowingServiceTests
    {
        private readonly Mock<IBorrowingRepository> _mockRepo;
        private readonly Mock<IMapper> _mockMapper;
        private readonly BorrowingService _service;

        public BorrowingServiceTests()
        {
            _mockRepo = new Mock<IBorrowingRepository>();
            _mockMapper = new Mock<IMapper>();
            _service = new BorrowingService(_mockRepo.Object, _mockMapper.Object);
        }

        [Fact]
        public async Task GetAllAsync_ReturnsMappedDtos()
        {
            // Arrange
            var borrowings = new List<Borrowing> { new Borrowing { Id = 1 } };
            var dtos = new List<BorrowingDTO> { new BorrowingDTO { Id = 1 } };
            _mockRepo.Setup(r => r.GetAllAsync()).ReturnsAsync(borrowings);
            _mockMapper.Setup(m => m.Map<IEnumerable<BorrowingDTO>>(borrowings)).Returns(dtos);

            // Act
            var result = await _service.GetAllAsync();

            // Assert
            Assert.Equal(dtos, result);
        }

        [Fact]
        public async Task GetByCompositeKeyAsync_ReturnsMappedDto()
        {
            // Arrange
            var borrowing = new Borrowing { Id = 1 };
            var dto = new BorrowingDTO { Id = 1 };
            _mockRepo.Setup(r => r.GetByCompositeKeyAsync(1, 2, "2024-01-01", "2024-01-10")).ReturnsAsync(borrowing);
            _mockMapper.Setup(m => m.Map<BorrowingDTO>(borrowing)).Returns(dto);

            // Act
            var result = await _service.GetByCompositeKeyAsync(1, 2, "2024-01-01", "2024-01-10");

            // Assert
            Assert.Equal(dto, result);
        }

        [Fact]
        public async Task CreateAsync_MapsAndAddsBorrowing()
        {
            // Arrange
            var dto = new BorrowingDTO { Id = 1 };
            var borrowing = new Borrowing { Id = 1 };
            _mockMapper.Setup(m => m.Map<Borrowing>(dto)).Returns(borrowing);

            // Act
            await _service.CreateAsync(dto);

            // Assert
            _mockRepo.Verify(r => r.AddAsync(borrowing), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_ExistingBorrowing_MapsAndUpdates()
        {
            // Arrange
            var dto = new BorrowingDTO { Id = 1 };
            var borrowing = new Borrowing { Id = 1 };
            _mockRepo.Setup(r => r.GetByCompositeKeyAsync(1, 2, "2024-01-01", "2024-01-10")).ReturnsAsync(borrowing);

            // Act
            await _service.UpdateAsync(1, 2, "2024-01-01", "2024-01-10", dto);

            // Assert
            _mockMapper.Verify(m => m.Map(dto, borrowing), Times.Once);
            _mockRepo.Verify(r => r.UpdateAsync(borrowing), Times.Once);
        }

        [Fact]
        public async Task UpdateAsync_NonExistingBorrowing_ThrowsException()
        {
            // Arrange
            var dto = new BorrowingDTO { Id = 1 };
            _mockRepo.Setup(r => r.GetByCompositeKeyAsync(1, 2, "2024-01-01", "2024-01-10")).ReturnsAsync((Borrowing)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _service.UpdateAsync(1, 2, "2024-01-01", "2024-01-10", dto));
        }

        [Fact]
        public async Task DeleteAsync_ExistingBorrowing_Deletes()
        {
            // Arrange
            var borrowing = new Borrowing { Id = 1 };
            _mockRepo.Setup(r => r.GetByCompositeKeyAsync(1, 2, "2024-01-01", "2024-01-10")).ReturnsAsync(borrowing);

            // Act
            await _service.DeleteAsync(1, 2, "2024-01-01", "2024-01-10");

            // Assert
            _mockRepo.Verify(r => r.DeleteAsync(borrowing), Times.Once);
        }

        [Fact]
        public async Task DeleteAsync_NonExistingBorrowing_ThrowsException()
        {
            // Arrange
            _mockRepo.Setup(r => r.GetByCompositeKeyAsync(1, 2, "2024-01-01", "2024-01-10")).ReturnsAsync((Borrowing)null);

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() =>
                _service.DeleteAsync(1, 2, "2024-01-01", "2024-01-10"));
        }
    }
