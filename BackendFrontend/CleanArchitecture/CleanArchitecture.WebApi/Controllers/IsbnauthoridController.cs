using CleanArchitecture.Core.DTOs.Isbnauthorid;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IsbnauthoridController : ControllerBase
    {
        private readonly IIsbnauthoridService _isbnauthoridService;

        public IsbnauthoridController(IIsbnauthoridService isbnauthoridService)
        {
            _isbnauthoridService = isbnauthoridService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _isbnauthoridService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{Id}/{authorid}")]
        public async Task<IActionResult> GetByCompositeKey(long Id, long authorid)
        {
            var result = await _isbnauthoridService.GetByCompositeKeyAsync(Id, authorid);
            if (result == null) return NotFound();
            return Ok(result);
        }


        [HttpPost]
        public async Task<IActionResult> Create([FromBody] IsbnauthoridDTO dto)
        {
            await _isbnauthoridService.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{Id}/{authorid}")]
        public async Task<IActionResult> Update(long Id, long authorid, [FromBody] IsbnauthoridDTO dto)
        {
            await _isbnauthoridService.UpdateAsync(Id, authorid, dto);
            return Ok();
        }


        [HttpDelete("{Id}/{authorid}")]
        public async Task<IActionResult> Delete(long Id, long authorid)
        {
            await _isbnauthoridService.DeleteAsync(Id, authorid);
            return Ok();
        }

    }
}
