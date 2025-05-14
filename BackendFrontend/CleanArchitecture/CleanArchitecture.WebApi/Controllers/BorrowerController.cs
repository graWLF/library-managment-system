using CleanArchitecture.Core.DTOs.Borrower;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Controllers

{
    [ApiController]
    [Route("api/[controller]")]
    public class BorrowerController : ControllerBase
    {
        private readonly IBorrowerService _borrowerService;

        public BorrowerController(IBorrowerService borrowerService)
        {
            _borrowerService = borrowerService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _borrowerService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(long id)
        {
            var result = await _borrowerService.GetByIDAsync(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BorrowerDTO dto)
        {
            await _borrowerService.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(long id, [FromBody] BorrowerDTO dto)
        {
            await _borrowerService.UpdateAsync(id, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            await _borrowerService.DeleteAsync(id);
            return Ok();
        }
    }
}
