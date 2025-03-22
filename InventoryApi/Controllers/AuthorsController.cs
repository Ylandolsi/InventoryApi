using InventoryApi.Models;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorsController : ControllerBase
    {
        private readonly IAuthorService _service;
        public AuthorsController(IAuthorService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            var authors = await _service.GetAllAsync();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id) 
        {
            var author = await _service.GetByIdAsync(id);
            if (author == null) return NotFound();
            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Author author) 
        {
            var id = await _service.AddAsync(author);
            return CreatedAtAction(nameof(GetById), new { id = id }, author);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Author author) 
        {
            author.Id = id;
            var result = await _service.UpdateAsync(author);
            if (result == 0) return NotFound();
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id) 
        {
            var result = await _service.DeleteAsync(id);
            if (result == 0) return NotFound();
            return NoContent();
        }


        [HttpGet("search")]
        public async Task<IActionResult> Search(string query)
        {
            var results = await _service.SearchAsync(query);
            return Ok(results);
        }
    }
}
