using Dapper;
using InventoryApi.Models;
using System.Data;

namespace InventoryApi.Services
{
    public interface IGenreService
    {
        Task<IEnumerable<GenreDto>> GetAllAsync();
        Task<Genre> GetByIdAsync(int id);
        Task<int> AddAsync(Genre genre);
        Task<int> UpdateAsync(Genre genre);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<GenreDto>> SearchAsync(string query);
    }

    public class GenreDto{
        public int Id{set;get; }
        public string Name{set;get;}
        public string Description{set;get;}
        public int BookCount{set;get;}
    }

    public class GenreService : IGenreService
    {
        private readonly IDbConnection _db;
        public GenreService(IDbConnection db) => _db = db;

        public async Task<IEnumerable<GenreDto>> GetAllAsync()
        {
            var sql = @"SELECT g.*, COUNT(b.id) AS BookCount 
                        FROM Genres g
                        LEFT JOIN Books b ON g.Id = b.GenreId
                        GROUP BY g.Id";
            return await _db.QueryAsync<GenreDto>(sql);
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
            var sql = "UPDATE Genres SET Name = @Name, Description = @Description WHERE Id = @Id";
            return await _db.ExecuteAsync(sql, genre);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Genres WHERE Id = @Id";
            return await _db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<GenreDto>> SearchAsync(string query)
        {
            var sql = @"SELECT g.*, COUNT(b.id) AS BookCount 
                    FROM Genres g
                    LEFT JOIN Books b ON g.Id = b.GenreId 
                    WHERE g.Name ILIKE @Query OR g.Description ILIKE @Query
                    GROUP BY g.Id";
            return await _db.QueryAsync<GenreDto>(sql, new { Query = $"%{query}%" });
        }
    }
}
