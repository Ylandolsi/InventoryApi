# Inventory API

A RESTful API for managing book inventory with PostgreSQL database integration, built with .NET 9.

## Features

- CRUD operations for Books, Authors, and Genres
- Search functionality across all entities
- Relational data management (books linked to authors and genres)
- Database seeding for initial data
- Dockerized deployment

## Tech Stack

- .NET 9
- PostgreSQL
- Dapper 
- Docker & Docker Compose

## Project Structure

- **Models**: Book, Author, Genre domain entities
- **Services**: Business logic implementation for each entity
- **Controllers**:  API endpoints
- **DbInitializer**: Database schema creation
- **SeedData**: Initial data population

## Getting Started

### Prerequisites

- .NET 9 SDK
- PostgreSQL or Docker

### Local Development

1. Clone the repository
2. Update connection string in `appsettings.json` if needed
3. Run the API:
   ```
   cd InventoryApi
   dotnet run
   ```
4. Access the API at http://localhost:5159

### Docker Deployment

```
docker-compose up -d
```

This will start both the API and PostgreSQL database in containers.

## API Endpoints

- **Books**: `/api/books`
- **Authors**: `/api/authors`
- **Genres**: `/api/genres`

Each endpoint supports:
- GET (all)
- GET /{id} (single)
- POST (create)
- PUT /{id} (update)
- DELETE /{id} (delete)
- GET /search?query={term} (search)

## Frontend Integration

The API is configured to accept CORS requests from:
- http://localhost:5173 (development frontend)
- https://inventory-frontend-peach.vercel.app (production frontend)
