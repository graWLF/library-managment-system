using CleanArchitecture.Core.DTOs.Registration;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.WebApi.Controllers;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace CleanArchitecture.WebApi.Tests.Controllers
{
    public class RegistrationControllerTests
    {
        private readonly Mock<IRegistrationService> _serviceMock;
        private readonly RegistrationController _controller;

        public RegistrationControllerTests()
        {
            _serviceMock = new Mock<IRegistrationService>();
            _controller = new RegistrationController(_serviceMock.Object);
        }

        [Fact]
        public async Task GetAll_ReturnsOkWithResult()
        {
            var dtos = new List<RegistrationDTO>
        {
            new RegistrationDTO { Id = 1, Username = "user1" },
            new RegistrationDTO { Id = 2, Username = "user2" }
        };
            _serviceMock.Setup(s => s.GetAllAsync()).ReturnsAsync(dtos);

            var result = await _controller.GetAll();

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dtos, okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsOkWithResult_WhenFound()
        {
            var dto = new RegistrationDTO { Id = 1, Username = "user1" };
            _serviceMock.Setup(s => s.GetByIDAsync(1)).ReturnsAsync(dto);

            var result = await _controller.GetById(1);

            var okResult = Assert.IsType<OkObjectResult>(result);
            Assert.Equal(dto, okResult.Value);
        }

        [Fact]
        public async Task GetById_ReturnsNotFound_WhenNotFound()
        {
            _serviceMock.Setup(s => s.GetByIDAsync(1)).ReturnsAsync((RegistrationDTO)null);

            var result = await _controller.GetById(1);

            var notFound = Assert.IsType<NotFoundObjectResult>(result);
            Assert.Contains("not found", notFound.Value.ToString().ToLower());
        }

        [Fact]
        public async Task Create_ReturnsBadRequest_WhenModelStateInvalid()
        {
            _controller.ModelState.AddModelError("Username", "Required");

            var result = await _controller.Create(new RegistrationDTO());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Create_ReturnsCreatedAtAction_WhenValid()
        {
            var dto = new RegistrationDTO { Id = 1, Username = "user1" };
            _serviceMock.Setup(s => s.CreateAsync(dto)).Returns(Task.CompletedTask);

            var result = await _controller.Create(dto);

            var created = Assert.IsType<CreatedAtActionResult>(result);
            Assert.Equal(nameof(_controller.GetById), created.ActionName);
            Assert.Equal(dto, created.Value);
        }

        [Fact]
        public async Task Update_ReturnsBadRequest_WhenModelStateInvalid()
        {
            _controller.ModelState.AddModelError("Username", "Required");

            var result = await _controller.Update(1, new RegistrationDTO());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Update_ReturnsNoContent_WhenValid()
        {
            var dto = new RegistrationDTO { Id = 1, Username = "user1" };
            _serviceMock.Setup(s => s.UpdateAsync(1, dto)).Returns(Task.CompletedTask);

            var result = await _controller.Update(1, dto);

            Assert.IsType<NoContentResult>(result);
            _serviceMock.Verify(s => s.UpdateAsync(1, dto), Times.Once);
        }

        [Fact]
        public async Task Delete_ReturnsNoContent()
        {
            _serviceMock.Setup(s => s.DeleteAsync(1)).Returns(Task.CompletedTask);

            var result = await _controller.Delete(1);

            Assert.IsType<NoContentResult>(result);
            _serviceMock.Verify(s => s.DeleteAsync(1), Times.Once);
        }

        [Fact]
        public async Task Login_ReturnsBadRequest_WhenModelStateInvalid()
        {
            _controller.ModelState.AddModelError("Username", "Required");

            var result = await _controller.Login(new RegistrationController.LoginRequestDTO());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task Login_ReturnsUnauthorized_WhenInvalidCredentials()
        {
            _serviceMock.Setup(s => s.Login("user", "wrong")).Returns(false);

            var result = await _controller.Login(new RegistrationController.LoginRequestDTO { Username = "user", Password = "wrong" });

            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Contains("invalid credentials", unauthorized.Value.ToString().ToLower());
        }

        [Fact]
        public async Task Login_ReturnsOk_WhenValidCredentials()
        {
            _serviceMock.Setup(s => s.Login("user", "pass")).Returns(true);

            var result = await _controller.Login(new RegistrationController.LoginRequestDTO { Username = "user", Password = "pass" });

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Login successful.", ok.Value);
        }

        [Fact]
        public async Task CheckSupervisor_ReturnsBadRequest_WhenModelStateInvalid()
        {
            _controller.ModelState.AddModelError("Username", "Required");

            var result = await _controller.CheckSupervisor(new RegistrationController.LoginRequestDTO());

            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public async Task CheckSupervisor_ReturnsUnauthorized_WhenNotSupervisor()
        {
            _serviceMock.Setup(s => s.CheckSupervisor("user", "pass")).Returns(false);

            var result = await _controller.CheckSupervisor(new RegistrationController.LoginRequestDTO { Username = "user", Password = "pass" });

            var unauthorized = Assert.IsType<UnauthorizedObjectResult>(result);
            Assert.Contains("unauthorized", unauthorized.Value.ToString().ToLower());
        }

        [Fact]
        public async Task CheckSupervisor_ReturnsOk_WhenSupervisor()
        {
            _serviceMock.Setup(s => s.CheckSupervisor("super", "pass")).Returns(true);

            var result = await _controller.CheckSupervisor(new RegistrationController.LoginRequestDTO { Username = "super", Password = "pass" });

            var ok = Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Supervisor login successful.", ok.Value);
        }
    }
}