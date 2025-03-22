using Dapper;
using InventoryApi.Models;
using System.Data;

namespace InventoryApi.Services
{
    public interface IBookService
    {
        Task<IEnumerable<Book>> GetAllAsync();
        Task<Book> GetByIdAsync(int id);
        Task<int> AddAsync(Book book);
        Task<int> UpdateAsync(Book book);
        Task<int> DeleteAsync(int id);
    }

    public class BookService : IBookService
    {
        private readonly IDbConnection _db;
        public BookService(IDbConnection db) => _db = db;

        public async Task<IEnumerable<Book>> GetAllAsync()
        {
            var sql = "SELECT * FROM Books";
            return await _db.QueryAsync<Book>(sql);
        }

        public async Task<Book> GetByIdAsync(int id)
        {
            var sql = "SELECT * FROM Books WHERE Id = @Id";
            return await _db.QueryFirstOrDefaultAsync<Book>(sql, new { Id = id });
        }

        public async Task<int> AddAsync(Book book)
        {
            var sql = "INSERT INTO Books (Title, AuthorId, GenreId, Quantity, Description) VALUES (@Title, @AuthorId, @GenreId, @Quantity, @Description) RETURNING Id";
            return await _db.ExecuteScalarAsync<int>(sql, book);
        }

        public async Task<int> UpdateAsync(Book book)
        {
            var sql = "UPDATE Books SET Title = @Title, AuthorId = @AuthorId, GenreId = @GenreId, Quantity = @Quantity , Description = @Description  WHERE Id = @Id";
            return await _db.ExecuteAsync(sql, book);
        }

        public async Task<int> DeleteAsync(int id)
        {
            var sql = "DELETE FROM Books WHERE Id = @Id";
            return await _db.ExecuteAsync(sql, new { Id = id });
        }
    }
}
