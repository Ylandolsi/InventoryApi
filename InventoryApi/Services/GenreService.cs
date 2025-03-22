using Dapper;
using InventoryApi.Models;
using System.Data;

namespace InventoryApi.Services
{
    public interface IGenreService
    {
        Task<IEnumerable<Genre>> GetAllAsync();
        Task<Genre> GetByIdAsync(int id);
        Task<int> AddAsync(Genre genre);
        Task<int> UpdateAsync(Genre genre);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<Genre>> SearchAsync(string query);
    }

    public class GenreService : IGenreService
    {
        private readonly IDbConnection _db;
        public GenreService(IDbConnection db) => _db = db;

        public async Task<IEnumerable<Genre>> GetAllAsync()
        {
            var sql = "SELECT * FROM Genres";
            return await _db.QueryAsync<Genre>(sql);
        }

        public async Task<Genre> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Genres WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Genre>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Genre genre)
        {
            var sql = "INSERT INTO Genres (Name, Description) VALUES (@Name, @Description) RETURNING Id";
            return await _db.ExecuteScalarAsync<int>(sql, genre);
        }

        public async Task<int> UpdateAsync(Genre genre)
        {
            var sql = "UPDATE Genres SET Name = @Name  , Description = @Description WHERE Id = @Id";
            return await _db.ExecuteAsync(sql, genre);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Genres WHERE Id = @Id";
            return await _db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<Genre>> SearchAsync(string query)
        {
            var sql = "SELECT * FROM Genres WHERE Name ILIKE @Query OR Description ILIKE @Query";
            return await _db.QueryAsync<Genre>(sql, new { Query = $"%{query}%" });
        }
    }
}
