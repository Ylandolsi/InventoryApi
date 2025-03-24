using Microsoft.AspNetCore.Mvc;
using System.Data;

using Dapper;

namespace InventoryApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ResetController : ControllerBase
    {
        private readonly IDbConnection _db;
        public ResetController(IDbConnection db) => _db = db;

        [HttpPost]
        public async Task<IActionResult> ResetAndSeed()
        {
            await _db.ExecuteAsync("DROP TABLE IF EXISTS Books;");
            await _db.ExecuteAsync("DROP TABLE IF EXISTS Authors;");
            await _db.ExecuteAsync("DROP TABLE IF EXISTS Genres;");
            
            await DbInitializer.InitializeAsync(_db);
            await SeedData.SeedAsync(_db);
            
            return Ok(new { message = "Database reset and seeded successfully" });
        }
    }
}
