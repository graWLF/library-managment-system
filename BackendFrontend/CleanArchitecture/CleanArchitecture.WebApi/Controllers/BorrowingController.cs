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

        [HttpGet("{itemNo}/{borrowerId}/{borrowDate}/{dueDate}")]
        public async Task<IActionResult> GetByCompositeKey(long itemNo, long borrowerId, string borrowDate, string dueDate)
        {
            var result = await _borrowingService.GetByCompositeKeyAsync(itemNo, borrowerId, borrowDate, dueDate);
            if (result == null) return NotFound();
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BorrowingDTO dto)
        {
            await _borrowingService.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{itemNo}/{borrowerId}/{borrowDate}/{dueDate}")]
        public async Task<IActionResult> Update(long itemNo, long borrowerId, string borrowDate, string dueDate, [FromBody] BorrowingDTO dto)
        {
            await _borrowingService.UpdateAsync(itemNo, borrowerId, borrowDate, dueDate, dto);
            return Ok();
        }


        [HttpDelete("{itemNo}/{borrowerId}/{borrowDate}/{dueDate}")]
        public async Task<IActionResult> Delete(long itemNo, long borrowerId, string borrowDate, string dueDate)
        {
            await _borrowingService.DeleteAsync(itemNo, borrowerId, borrowDate, dueDate);
            return Ok();
        }

    }
}
