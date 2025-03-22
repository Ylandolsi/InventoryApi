using System.Data;
using System.Threading.Tasks;
using Dapper;

namespace InventoryApi
{
    public static class DbInitializer
    {
        public static async Task InitializeAsync(IDbConnection db)
        {
            await db.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Authors (
                    Id SERIAL PRIMARY KEY,
                    Name VARCHAR(100) NOT NULL,
                    Bio TEXT
                );");

            await db.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Genres (
                    Id SERIAL PRIMARY KEY,
                    Name VARCHAR(50) NOT NULL,
                    Description TEXT
                );");

            await db.ExecuteAsync(@"
                CREATE TABLE IF NOT EXISTS Books (
                    Id SERIAL PRIMARY KEY,
                    Title VARCHAR(200) NOT NULL,
                    Quantity INT NOT NULL DEFAULT 0,
                    AuthorId INT REFERENCES Authors(Id),
                    GenreId INT REFERENCES Genres(Id),
                    Description TEXT
                );");
        }
    }
}