using Dapper;
using InventoryApi.Models;
using System.Data;

namespace InventoryApi.Services
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Bio { get; set; }
        public int BookCount { get; set; }

    }
    public interface IAuthorService
    {
        Task<IEnumerable<AuthorDto>> GetAllAsync();
        Task<AuthorDto> GetByIdAsync(int id);
        Task<int> AddAsync(Author author);
        Task<int> UpdateAsync(Author author);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<AuthorDto>> SearchAsync(string query);
    }



    public class AuthorService : IAuthorService
    {
        private readonly IDbConnection _db;
        public AuthorService(IDbConnection db) => _db = db;

        public async Task<IEnumerable<AuthorDto>> GetAllAsync()
        {
            var sql = @"SELECT a.*,  COUNT(b.Id) AS BookCount  
                        FROM Authors  a 
                        LEFT JOIN Books  b ON a.Id = b.AuthorId
                        GROUP BY a.Id";
            return await _db.QueryAsync<AuthorDto>(sql);
        }

        public async Task<AuthorDto> GetByIdAsync(int id)
        {
            var sql = @"SELECT a.*  , COUNT ( b.Id) AS BookCount 
                        FROM Authors a 
                        LEFT JOIN Books b on a.Id = b.AuthorId
                        WHERE a.Id = @Id
                        GROUP BY a.Id ";
            return await _db.QueryFirstOrDefaultAsync<AuthorDto>(sql, new { Id = id });
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

        public async Task<IEnumerable<AuthorDto>> SearchAsync(string query)
        {
            var sql = @"SELECT a.*,  COUNT(b.Id) AS BookCount 
            FROM Authors  a 
            LEFT JOIN Books  b ON a.Id = b.AuthorId
            WHERE Name ILIKE @Query OR Bio ILIKE @Query
            GROUP BY a.Id";
   
            return await _db.QueryAsync<AuthorDto>(sql, new { Query = $"%{query}%" });
        }

        
    }
}
