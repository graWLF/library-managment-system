using CleanArchitecture.Core.DTOs.Supervisor;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SupervisorController : ControllerBase
    {
        private readonly ISupervisorService _supervisorService;

        public SupervisorController(ISupervisorService supervisorService)
        {
            _supervisorService = supervisorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _supervisorService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var result = await _supervisorService.GetByIDAsync(ID);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] SupervisorDTO dto)
        {
            await _supervisorService.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int ID, [FromBody] SupervisorDTO dto)
        {
            await _supervisorService.UpdateAsync(ID, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int ID)
        {
            await _supervisorService.DeleteAsync(ID);
            return Ok();
        }
    }
}
