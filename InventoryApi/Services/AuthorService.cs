using Dapper;
using InventoryApi.Models;
using System.Data;

namespace InventoryApi.Services
{
    public interface IAuthorService
    {
        Task<IEnumerable<Author>> GetAllAsync();
        Task<Author> GetByIdAsync(int id);
        Task<int> AddAsync(Author author);
        Task<int> UpdateAsync(Author author);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<Author>> SearchAsync(string query);
    }

    public class AuthorService : IAuthorService
    {
        private readonly IDbConnection _db;
        public AuthorService(IDbConnection db) => _db = db;

        public async Task<IEnumerable<Author>> GetAllAsync()
        {
            var sql = "SELECT * FROM Authors";
            return await _db.QueryAsync<Author>(sql);
        }

        public async Task<Author> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Authors WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Author>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Author author)
        {
            var sql = "INSERT INTO Authors (Name, Bio) VALUES (@Name, @Bio) RETURNING Id";
            return await _db.ExecuteScalarAsync<int>(sql, author);
        }

        public async Task<int> UpdateAsync(Author author)
        {
            var sql = "UPDATE Authors SET Name = @Name , Bio = @Bio WHERE Id = @Id";
            return await _db.ExecuteAsync(sql, author);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Authors WHERE Id = @Id";
            return await _db.ExecuteAsync(sql, new { Id = id });
        }

        public async Task<IEnumerable<Author>> SearchAsync(string query)
        {
            var sql = "SELECT * FROM Authors WHERE Name ILIKE @Query OR Bio ILIKE @Query";
            return await _db.QueryAsync<Author>(sql, new { Query = $"%{query}%" });
        }

        
    }
}
