using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace InventoryApi
{
    public static class SeedData
    {
        public static async Task SeedAsync(IDbConnection db)
        {
            var authorCount = await db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Authors");
            if(authorCount == 0)
            {
                await db.ExecuteAsync("INSERT INTO Authors (Name, Bio) VALUES (@Name, @Bio)",
                    new[]
                    {
                        new { Name = "Author One", Bio = "Bio for author one" },
                        new { Name = "Author Two", Bio = "Bio for author two" }
                    });
            }
            
            var genreCount = await db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Genres");
            if(genreCount == 0)
            {
                await db.ExecuteAsync("INSERT INTO Genres (Name, Description) VALUES (@Name, @Description)",
                    new[]
                    {
                        new { Name = "Genre One", Description = "Description for genre one" },
                        new { Name = "Genre Two", Description = "Description for genre two" }
                    });
            }
            
            var bookCount = await db.ExecuteScalarAsync<int>("SELECT COUNT(*) FROM Books");
            if(bookCount == 0)
            {
                await db.ExecuteAsync("INSERT INTO Books (Title, AuthorId, GenreId, Quantity, Description) VALUES (@Title, @AuthorId, @GenreId, @Quantity, @Description)",
                    new[]
                    {
                        new { Title = "Book One", AuthorId = 1, GenreId = 1, Quantity = 10, Description = "Description for book one" },
                        new { Title = "Book Two", AuthorId = 2, GenreId = 2, Quantity = 5, Description = "Description for book two" }
                    });
            }
        }
    }
}