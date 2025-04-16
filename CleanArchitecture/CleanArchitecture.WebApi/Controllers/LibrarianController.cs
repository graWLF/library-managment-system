using CleanArchitecture.Core.DTOs.Librarian;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LibrarianController : ControllerBase
    {
        private readonly ILibrarianService _librarianService;

        public LibrarianController(ILibrarianService librarianService)
        {
            _librarianService = librarianService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _librarianService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var result = await _librarianService.GetByIDAsync(ID);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] LibrarianDTO dto)
        {
            await _librarianService.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int ID, [FromBody] LibrarianDTO dto)
        {
            await _librarianService.UpdateAsync(ID, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int ID)
        {
            await _librarianService.DeleteAsync(ID);
            return Ok();
        }
    }
}
