using Dapper;
using InventoryApi.Models;
using System.Data;

namespace InventoryApi.Services
{
    public class BookDto
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public int Quantity { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Genre{ get; set; }
    }

    public interface IBookService
    {
        Task<IEnumerable<BookDto>> GetAllAsync();
        Task<BookDto> GetByIdAsync(int id);
        Task<int> AddAsync(Book book);
        Task<int> UpdateAsync(Book book);
        Task<int> DeleteAsync(int id);
        Task<IEnumerable<Book>> SearchAsync(string query);
        Task<IEnumerable<BookDto>> SearchWithDetailsAsync(string searchTerm);
    }


    public class BookService : IBookService
    {
        private readonly IDbConnection _db;
        public BookService(IDbConnection db) => _db = db;


        public async Task<IEnumerable<BookDto>> GetAllAsync()
        {
            const string sql = @"
                SELECT b.Id, 
                    b.Title, 
                    b.Quantity, 
                    b.Description, 
                    a.Name AS author, 
                    g.Name AS genre 
                FROM Books b
                LEFT JOIN Authors a ON b.AuthorId = a.Id 
                LEFT JOIN Genres g ON b.GenreId = g.Id";

            var results = await _db.QueryAsync<BookDto>(sql);

            return results;
        }

        public async Task<BookDto> GetByIdAsync(int id)
        {
            var sql = @"SELECT b.*, a.Name AS author, g.Name AS genre 
                      FROM Books b
                      LEFT JOIN Authors a ON b.AuthorId = a.Id 
                      LEFT JOIN Genres g ON b.GenreId = g.Id
                      WHERE b.Id = @Id";
            var result = await _db.QuerySingleOrDefaultAsync<BookDto>(sql, new { Id = id });

            return result ;
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

        public async Task<IEnumerable<Book>> SearchAsync(string query)
        {
            var sql = "SELECT * FROM Books WHERE Title ILIKE @Query OR Description ILIKE @Query";
            return await _db.QueryAsync<Book>(sql, new { Query = $"%{query}%" });
        }

        public async Task<IEnumerable<BookDto>> SearchWithDetailsAsync(string searchTerm)
        {
            var query = @"
                SELECT b.*, a.Name as author, 
                        g.Name as genre 
                FROM Books b
                LEFT JOIN Authors a ON b.AuthorId = a.Id
                LEFT JOIN Genres g ON b.GenreId = g.Id
                WHERE b.Title ILIKE @SearchPattern OR b.Description ILIKE @SearchPattern";
            
            var results = await _db.QueryAsync<BookDto>(query, new { SearchPattern = $"%{searchTerm}%" });
            
            return results;
        }
    }
}
