using CleanArchitecture.Core.DTOs.Borrowing;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BorrowingController : ControllerBase
    {
        private readonly IBorrowingService _borrowingService;

        public BorrowingController(IBorrowingService borrowingService)
        {
            _borrowingService = borrowingService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _borrowingService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{isbn}")]
        public async Task<IActionResult> GetByISBN(long ID)
        {
            var result = await _borrowingService.GetByIDAsync(ID);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BorrowingDTO dto)
        {
            await _borrowingService.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{isbn}")]
        public async Task<IActionResult> Update(long ID, [FromBody] BorrowingDTO dto)
        {
            await _borrowingService.UpdateAsync(ID, dto);
            return Ok();
        }

        [HttpDelete("{isbn}")]
        public async Task<IActionResult> Delete(long ID)
        {
            await _borrowingService.DeleteAsync(ID);
            return Ok();
        }
    }
}
