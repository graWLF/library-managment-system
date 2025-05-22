using AutoMapper;
using CleanArchitecture.Core.DTOs.Branch;
using CleanArchitecture.Core.Entities;
using CleanArchitecture.Core.Interfaces.Repositories;
using CleanArchitecture.Infrastructure.Services;
using Moq;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

public class BranchServiceTests
{
    private readonly Mock<IBranchRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly BranchService _service;

    public BranchServiceTests()
    {
        _repositoryMock = new Mock<IBranchRepository>();
        _mapperMock = new Mock<IMapper>();
        _service = new BranchService(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAllAsync_ReturnsMappedBranchDTOs()
    {
        var branches = new List<Branch> { new Branch { Id = 1 }, new Branch { Id = 2 } };
        var branchDTOs = new List<BranchDTO> { new BranchDTO { Id = 1 }, new BranchDTO { Id = 2 } };

        _repositoryMock.Setup(r => r.GetAllAsync()).ReturnsAsync(branches);
        _mapperMock.Setup(m => m.Map<IEnumerable<BranchDTO>>(branches)).Returns(branchDTOs);

        var result = await _service.GetAllAsync();

        Assert.Equal(branchDTOs, result);
    }

    [Fact]
    public async Task GetByIDAsync_ReturnsMappedBranchDTO()
    {
        var branch = new Branch { Id = 1 };
        var branchDTO = new BranchDTO { Id = 1 };

        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(branch);
        _mapperMock.Setup(m => m.Map<BranchDTO>(branch)).Returns(branchDTO);

        var result = await _service.GetByIDAsync(1);

        Assert.Equal(branchDTO, result);
    }

    [Fact]
    public async Task CreateAsync_MapsAndAddsBranch()
    {
        var branchDTO = new BranchDTO { Id = 1 };
        var branch = new Branch { Id = 1 };

        _mapperMock.Setup(m => m.Map<Branch>(branchDTO)).Returns(branch);

        await _service.CreateAsync(branchDTO);

        _repositoryMock.Verify(r => r.AddAsync(branch), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_ExistingBranch_MapsAndUpdates()
    {
        var branchDTO = new BranchDTO { Id = 1 };
        var existingBranch = new Branch { Id = 1 };

        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(existingBranch);

        await _service.UpdateAsync(1, branchDTO);

        _mapperMock.Verify(m => m.Map(branchDTO, existingBranch), Times.Once);
        _repositoryMock.Verify(r => r.UpdateAsync(existingBranch), Times.Once);
    }

    [Fact]
    public async Task UpdateAsync_NonExistingBranch_ThrowsException()
    {
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync((Branch)null);

        await Assert.ThrowsAsync<Exception>(() => _service.UpdateAsync(1, new BranchDTO()));
    }

    [Fact]
    public async Task DeleteAsync_ExistingBranch_DeletesBranch()
    {
        var branch = new Branch { Id = 1 };
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync(branch);

        await _service.DeleteAsync(1);

        _repositoryMock.Verify(r => r.DeleteAsync(branch), Times.Once);
    }

    [Fact]
    public async Task DeleteAsync_NonExistingBranch_ThrowsException()
    {
        _repositoryMock.Setup(r => r.GetByIDAsync(1)).ReturnsAsync((Branch)null);

        await Assert.ThrowsAsync<Exception>(() => _service.DeleteAsync(1));
    }
}