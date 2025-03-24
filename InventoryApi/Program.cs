using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Npgsql;
using System.Data;
using InventoryApi.Services;
using InventoryApi; 

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddScoped<IDbConnection>(sp =>
	new NpgsqlConnection(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddScoped<IBookService, BookService>();
builder.Services.AddScoped<IAuthorService, AuthorService>();
builder.Services.AddScoped<IGenreService, GenreService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowSpecificOrigins",
        builder =>
        {
            builder.WithOrigins(
                    "http://localhost:5173",      
                    "http://localhost:5159",     // Add the API's port for direct browser access
                    "https://inventory-api-la8y.onrender.com",     
                    "https://inventory-frontend-peach.vercel.app")     // Production domain
                .AllowAnyHeader()
                .AllowAnyMethod();
        });
});
var app = builder.Build();

// Initialize the database with a retry mechanism for both Development and Production
if (app.Environment.IsDevelopment() || app.Environment.IsProduction())
{
    var maxRetryAttempts = 5;
    var retryDelaySeconds = 10;
    
    for (int retryAttempt = 1; retryAttempt <= maxRetryAttempts; retryAttempt++)
    {
        try
        {
            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<IDbConnection>();
                await DbInitializer.InitializeAsync(db);
                await SeedData.SeedAsync(db);
                break; // If successful, break out of the retry loop
            }
        }
        catch (Exception ex)
        {
            if (retryAttempt == maxRetryAttempts)
            {
                Console.WriteLine($"FATAL DATABASE ERROR: {ex.Message}");
                Console.WriteLine($"Connection string: {builder.Configuration.GetConnectionString("DefaultConnection")?.Replace("Password=", "Password=***")}");
                Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
                Console.WriteLine($"Stack trace: {ex.StackTrace}");
                if (app.Environment.IsDevelopment())
                    throw;
            }
            
            Console.WriteLine($"Database initialization failed (Attempt {retryAttempt}/{maxRetryAttempts}): {ex.Message}");
            Console.WriteLine($"Retrying in {retryDelaySeconds} seconds...");
            await Task.Delay(TimeSpan.FromSeconds(retryDelaySeconds));
        }
    }
}

if (!app.Environment.IsDevelopment() && !app.Environment.IsEnvironment("Docker"))
{
    app.UseHsts();
}

if (app.Environment.IsDevelopment())
{
    app.UseHttpsRedirection();
}
app.UseCors("AllowSpecificOrigins");

app.UseAuthorization();
app.MapControllers();
app.Run();

