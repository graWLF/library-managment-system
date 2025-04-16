using CleanArchitecture.Core.DTOs.Publisher;
using CleanArchitecture.Core.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace CleanArchitecture.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PublisherController : ControllerBase
    {
        private readonly IPublisherService _publisherService;

        public PublisherController(IPublisherService publisherService)
        {
            _publisherService = publisherService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _publisherService.GetAllAsync();
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetByID(int ID)
        {
            var result = await _publisherService.GetByIDAsync(ID);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] PublisherDTO dto)
        {
            await _publisherService.CreateAsync(dto);
            return Ok();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int ID, [FromBody] PublisherDTO dto)
        {
            await _publisherService.UpdateAsync(ID, dto);
            return Ok();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int ID)
        {
            await _publisherService.DeleteAsync(ID);
            return Ok();
        }
    }
}
