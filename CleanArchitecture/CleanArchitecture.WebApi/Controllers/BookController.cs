using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.DTOs.Book;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bookService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{isbn}")]
        public async Task<IActionResult> GetByISBN(long isbn)
        {
            var result = await _bookService.GetByISBNAsync(isbn);
            return Ok(result);
        }

        [HttpGet("search")]
        public async Task<IActionResult> GetByName([FromQuery] string name)
        {
            var result = await _bookService.GetByNameAsync(name);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookDto dto)
        {
            await _bookService.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{isbn}")]
        public async Task<IActionResult> Update(long isbn, [FromBody] BookDto dto)
        {
            await _bookService.UpdateAsync(isbn, dto);
            return Ok();
        }

        [HttpDelete("{isbn}")]
        public async Task<IActionResult> Delete(long isbn)
        {
            await _bookService.DeleteAsync(isbn);
            return Ok();
        }
    }

}
