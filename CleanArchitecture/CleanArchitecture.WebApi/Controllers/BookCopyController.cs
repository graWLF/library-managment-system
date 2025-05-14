using CleanArchitecture.Core.DTOs.BookCopy;
using CleanArchitecture.Core.Interfaces;
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
        [HttpGet("{itemNo}/{isbn}")]
        public async Task<IActionResult> GetByCompositeKey(long itemNo, long isbn)
        {
            var result = await _bookCopyService.GetByCompositeKeyAsync(itemNo, isbn);
            if (result == null) return NotFound();
            return Ok(result);
        }
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookCopyDTO dto)
        {
            await _bookCopyService.CreateAsync(dto);
            return Ok();
        }
        [HttpPut("{itemNo}/{isbn}")]

        public async Task<IActionResult> Update(long itemNo, long copyNo, [FromBody] BookCopyDTO dto)
        {
            await _bookCopyService.UpdateAsync(itemNo, copyNo, dto);
            return Ok();
        }
        [HttpDelete("{itemNo}/{isbn}")]
        public async Task<IActionResult> Delete(long itemNo, long copyNo)
        {
            await _bookCopyService.DeleteAsync(itemNo, copyNo);
            return Ok();
        }
    }
}
