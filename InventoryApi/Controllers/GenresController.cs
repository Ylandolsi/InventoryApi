using InventoryApi.Models;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GenresController : ControllerBase
    {
        private readonly IGenreService _service;
        public GenresController(IGenreService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get() 
        {
            var genres = await _service.GetAllAsync();
            return Ok(genres);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id) 
        {
            var genre = await _service.GetByIdAsync(id);
            if (genre == null) return NotFound();
            return Ok(genre);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Genre genre) 
        {
            var id = await _service.AddAsync(genre);
            return CreatedAtAction(nameof(Get), new { id = id }, genre);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Genre genre) 
        {
            genre.Id = id;
            var result = await _service.UpdateAsync(genre);
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
