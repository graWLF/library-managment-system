using CleanArchitecture.Core.DTOs.Author;
using CleanArchitecture.Core.DTOs.Book;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _authorService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{isbn}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var result = await _authorService.GetByIDAsync(ID);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorDTO dto)
        {
            await _authorService.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{isbn}")]
        public async Task<IActionResult> Update(int ID, [FromBody] AuthorDTO dto)
        {
            await _authorService.UpdateAsync(ID, dto);
            return Ok();
        }

        [HttpDelete("{isbn}")]
        public async Task<IActionResult> Delete(int ID)
        {
            await _authorService.DeleteAsync(ID);
            return Ok();
        }
    }
}
