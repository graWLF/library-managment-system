using Xunit;
using Moq;
using AutoMapper;
using CleanArchitecture.Infrastructure.Services;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Core.DTOs.Borrower;
using CleanArchitecture.Core.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;

public class BorrowerServiceTests
{
    private readonly Mock<IBorrowerRepository> _repoMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly BorrowerService _service;

    public BorrowerServiceTests()
    {
        _repoMock = new Mock<IBorrowerRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new BorrowerService(_repoMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedDtos()
    {
        var borrowers = new List<Borrower> { new Borrower { Id = 1, borrowername = "Ali", borrowerphone = "123" } };
        var dtos = new List<BorrowerDTO> { new BorrowerDTO { Id = 1, borrowerName = "Ali", borrowerPhone = "123" } };
        _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(borrowers);
        _mapperMock.Setup(m => m.Map<IEnumerable<BorrowerDTO>>(borrowers)).Returns(dtos);

        var result = await _service.GetAllAsync();

        Assert.Equal(dtos, result);
    }

    [Fact]
    public async Task GetByIDAsync_ReturnsMappedDto()
    {
        var borrower = new Borrower { Id = 2, borrowername = "Veli", borrowerphone = "456" };
        var dto = new BorrowerDTO { Id = 2, borrowerName = "Veli", borrowerPhone = "456" };
        _repoMock.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(borrower);
        _mapperMock.Setup(m => m.Map<BorrowerDTO>(borrower)).Returns(dto);

        var result = await _service.GetByIDAsync(2);

        Assert.Equal(dto, result);
    }

    [Fact]
    public async Task CreateAsync_MapsAndAddsBorrower()
    {
        var dto = new BorrowerDTO { Id = 3, borrowerName = "Ayşe", borrowerPhone = "789" };
        var borrower = new Borrower { Id = 3, borrowername = "Ayşe", borrowerphone = "789" };
        _mapperMock.Setup(m => m.Map<Borrower>(dto)).Returns(borrower);

        await _service.CreateAsync(dto);

        _repoMock.Verify(r => r.AddAsync(borrower), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ThrowsIfBorrowerNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(4)).ReturnsAsync((Borrower)null);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(4, new BorrowerDTO()));
    }

    [Fact]
    public async Task UpdateAsync_MapsAndUpdatesBorrower()
    {
        var existing = new Borrower { Id = 5, borrowername = "Eski", borrowerphone = "000" };
        var dto = new BorrowerDTO { Id = 5, borrowerName = "Yeni", borrowerPhone = "111" };
        _repoMock.Setup(r => r.GetByIdAsync(5)).ReturnsAsync(existing);

        await _service.UpdateAsync(5, dto);

        _mapperMock.Verify(m => m.Map(dto, existing), Times.Once);
        _repoMock.Verify(r => r.UpdateAsync(existing), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_ThrowsIfBorrowerNotFound()
    {
        _repoMock.Setup(r => r.GetByIdAsync(6)).ReturnsAsync((Borrower)null);

        await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(6));
    }

    [Fact]
    public async Task DeleteAsync_DeletesBorrower()
    {
        var borrower = new Borrower { Id = 7, borrowername = "Silinecek", borrowerphone = "222" };
        _repoMock.Setup(r => r.GetByIdAsync(7)).ReturnsAsync(borrower);

        await _service.DeleteAsync(7);

        _repoMock.Verify(r => r.DeleteAsync(borrower), Times.Once);
    }
}