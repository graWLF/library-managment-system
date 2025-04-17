using CleanArchitecture.Core.DTOs.Branch;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _branchService;

        public BranchController(IBranchService branchService)
        {
            _branchService = branchService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _branchService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var result = await _branchService.GetByIDAsync(ID);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BranchDTO dto)
        {
            await _branchService.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int ID, [FromBody] BranchDTO dto)
        {
            await _branchService.UpdateAsync(ID, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int ID)
        {
            await _branchService.DeleteAsync(ID);
            return Ok();
        }
    }
}
