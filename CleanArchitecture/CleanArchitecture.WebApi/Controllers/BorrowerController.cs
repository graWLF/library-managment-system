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

        [HttpGet("{isbn}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var result = await _borrowerService.GetByIDAsync(ID);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BorrowerDTO dto)
        {
            await _borrowerService.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{isbn}")]
        public async Task<IActionResult> Update(int ID, [FromBody] BorrowerDTO dto)
        {
            await _borrowerService.UpdateAsync(ID, dto);
            return Ok();
        }

        [HttpDelete("{isbn}")]
        public async Task<IActionResult> Delete(int ID)
        {
            await _borrowerService.DeleteAsync(ID);
            return Ok();
        }
    }
}
