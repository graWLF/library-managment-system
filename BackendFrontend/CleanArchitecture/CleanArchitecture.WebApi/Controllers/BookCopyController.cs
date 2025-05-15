using CleanArchitecture.Core.DTOs.BookCopy;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookCopyController : ControllerBase
    {
        private readonly IBookCopyService _bookCopyService;

        public BookCopyController(IBookCopyService bookCopyService)
        {
            _bookCopyService = bookCopyService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bookCopyService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{Id}")]
        public async Task<IActionResult> GetByID(long Id)
        {
            var result = await _bookCopyService.GetByIdAsync(Id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookCopyDTO dto)
        {
            await _bookCopyService.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{Id}")]
        public async Task<IActionResult> Update(long Id, [FromBody] BookCopyDTO dto)
        {
            await _bookCopyService.UpdateAsync(Id, dto);
            return Ok();
        }

        [HttpDelete("{Id}")]
        public async Task<IActionResult> Delete(long Id)
        {
            await _bookCopyService.DeleteAsync(Id);
            return Ok();
        }
    }
}
