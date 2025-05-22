using CleanArchitecture.Core.DTOs.Supervisor;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.WebApi.Tests.Controllers
{
    public class SupervisorControllerTests
    {
        private readonly Mock<ISupervisorService> _serviceMock;
        private readonly SupervisorController _controller;

        public SupervisorControllerTests()
        {
            _serviceMock = new Mock<ISupervisorService>();
            _controller = new SupervisorController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithResult()
        {
            var dtos = new List<SupervisorDTO>
        {
            new SupervisorDTO { Id = 1, SupervisorName = "A" },
            new SupervisorDTO { Id = 2, SupervisorName = "B" }
        };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(dtos);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtos, okResult.Value);
        }

        [Fact]
        public async Task GetByID_ReturnsOkWithResult()
        {
            var dto = new SupervisorDTO { Id = 1, SupervisorName = "A" };
            _serviceMock.Setup(s => s.GetByIDAsync(1)).ReturnsAsync(dto);

            var result = await _controller.GetByID(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dto, okResult.Value);
        }

        [Fact]
        public async Task Create_CallsServiceAndReturnsOk()
        {
            var dto = new SupervisorDTO { Id = 1, SupervisorName = "A" };
            _serviceMock.Setup(s => s.CreateAsync(dto)).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            Assert.IsType<OkResult>(result);
            _serviceMock.Verify(s => s.CreateAsync(dto), Times.Once);
        }

        [Fact]
        public async Task Update_CallsServiceAndReturnsOk()
        {
            var dto = new SupervisorDTO { Id = 1, SupervisorName = "A" };
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