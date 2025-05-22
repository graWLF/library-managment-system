using CleanArchitecture.Core.DTOs.Branch;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.WebApi.Tests.Controllers
{
    public class BranchControllerTests
    {
        private readonly Mock<IBranchService> _serviceMock;
        private readonly BranchController _controller;

        public BranchControllerTests()
        {
            _serviceMock = new Mock<IBranchService>();
            _controller = new BranchController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithResult()
        {
            var branches = new List<BranchDTO> { new BranchDTO { Id = 1 }, new BranchDTO { Id = 2 } };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(branches);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(branches, okResult.Value);
        }

        [Fact]
        public async Task GetByID_ReturnsOkWithResult()
        {
            var branch = new BranchDTO { Id = 1 };
            _serviceMock.Setup(s => s.GetByIDAsync(1)).ReturnsAsync(branch);

            var result = await _controller.GetByID(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(branch, okResult.Value);
        }

        [Fact]
        public async Task Create_CallsServiceAndReturnsOk()
        {
            var dto = new BranchDTO { Id = 1 };

            _serviceMock.Setup(s => s.CreateAsync(dto)).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.CreateAsync(dto), Times.Once);
        }

        [Fact]
        public async Task Update_CallsServiceAndReturnsOk()
        {
            var dto = new BranchDTO { Id = 1 };

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