using CleanArchitecture.Core.DTOs.Registration;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RegistrationController : ControllerBase
    {
        private readonly IRegistrationService _registrationService;

        public RegistrationController(IRegistrationService registrationService)
        {
            _registrationService = registrationService;
        }

        // GET: api/registration
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _registrationService.GetAllAsync();
            return Ok(result);
        }

        // GET: api/registration/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var result = await _registrationService.GetByIDAsync(id);
            if (result == null)
                return NotFound($"Registration with ID {id} not found.");
            return Ok(result);
        }

        // POST: api/registration
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] RegistrationDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _registrationService.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = dto.Id }, dto);
        }

        // PUT: api/registration/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] RegistrationDTO dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            await _registrationService.UpdateAsync(id, dto);
            return NoContent(); // Use 204 No Content for successful PUT
        }

        // DELETE: api/registration/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _registrationService.DeleteAsync(id);
            return NoContent(); // Use 204 No Content for successful DELETE
        }
    }
}
