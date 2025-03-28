using InventoryApi.Models;
using InventoryApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BooksController : ControllerBase
    {
        private readonly IBookService _service;
        public BooksController(IBookService service) => _service = service;

        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var books = await _service.GetAllAsync();
            return Ok(books);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var book = await _service.GetByIdAsync(id);
            if (book == null) return NotFound();
            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Book book)
        {
            var id = await _service.AddAsync(book);
            return CreatedAtAction(nameof(GetById), new { id }, book);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, [FromBody] Book book)
        {
            book.Id = id;
            var result = await _service.UpdateAsync(book);
            if (result == 0) return NotFound();
            return RedirectToAction(nameof(GetById), new { id });
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var result = await _service.DeleteAsync(id);
            if (result == 0) return NotFound();
            return NoContent();
        }
        
        [HttpGet("search")]
        public async Task<IActionResult> Search( [FromQuery]string query, bool includeDetails = true)
        {
            if (includeDetails)
            {
                var booksWithDetails = await _service.SearchWithDetailsAsync(query);
                return Ok(booksWithDetails);
            }
            else
            {
                var results = await _service.SearchAsync(query);
                return Ok(results);
            }
        }
    }
}