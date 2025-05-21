using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using CleanArchitecture.Core.Interfaces;
using CleanArchitecture.Core.DTOs.Book;
using System;
using Microsoft.AspNetCore.Http;

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
        public class Base64ImageRequest
        {
            public string ImageBase64 { get; set; } = string.Empty;
        }

        [HttpPost("extract-isbn-base64")]
        public async Task<IActionResult> ScanBarcode([FromBody] Base64ImageRequest request)
        {
            if (string.IsNullOrWhiteSpace(request.ImageBase64))
                return BadRequest("Image is required.");

            var result = await _bookService.ScanBarcodePathAsync(request.ImageBase64);
            return Ok(new { barcode = result });
        }


        [HttpPost("extract-isbn-path")]
        public async Task<IActionResult> ScanBarcode(string path)
        {
            if (path == null || path.Length == 0)
                return BadRequest("Image is required.");
            try
            {
                var result = await _bookService.ScanBarcodePathAsync(path);
                if (string.IsNullOrWhiteSpace(result))
                    return NotFound("No barcode found.");
                return Ok(new { barcode = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpPost("extract-isbn")]
        public async Task<IActionResult> ScanBarcode(IFormFile image)
        {
            if (image == null || image.Length == 0)
                return BadRequest("Image is required.");
            try
            {
                var result = await _bookService.ScanBarcodeAsync(image);
                if (string.IsNullOrWhiteSpace(result))
                    return NotFound("No barcode found.");
                return Ok(new { barcode = result });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = ex.Message, StackTrace = ex.StackTrace });
            }
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _bookService.GetAllAsync();
            return Ok(result);
        }
        [HttpGet("{isbn}/{apiKey}")]
        public async Task<IActionResult> SearchBookWeb(string isbn, string apiKey)
        {
            try
            {
                var book = await _bookService.SearchBookAsync(isbn, apiKey);
                return Ok(book);
            }
            catch (Exception ex)
            {
                // Log the error here
                return StatusCode(500, new { Message = ex.Message, StackTrace = ex.StackTrace });
            }
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
            return Ok(new { });
        }

        [HttpPut("{isbn}")]
        public async Task<IActionResult> Update(long isbn, [FromBody] BookDto dto)
        {
            await _bookService.UpdateAsync(isbn, dto);
            return Ok(new { });
        }

        [HttpDelete("{isbn}")]
        public async Task<IActionResult> Delete(long isbn)
        {
            await _bookService.DeleteAsync(isbn);
            return Ok(new { });
        }
    }

}
