using Microsoft.EntityFrameworkCore;
using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Core.Documents.Users;
using MovieDatabase.Api.Core.Utils;

using User = MovieDatabase.Api.Core.Documents.Users.User;

namespace MovieDatabase.Api.Infrastructure.Db;

public static class CosmosSeeder
{
    public static async Task SeedAsync(AppDbContext context)
    {
        // Check if data already exists by trying to find the admin user
        // This uses the partition key (Email) which is more efficient with Cosmos DB
        var existingAdmin = await context.Users
            .Where(u => u.Email == "admin@example.com")
            .FirstOrDefaultAsync();
            
        if (existingAdmin != null)
        {
            return; // Database is already seeded
        }

        // Seed users first
        var users = await SeedUsersAsync(context);
        
        // Seed films with admin user
        var adminUser = users.First(x => x.Role == UserRoles.Administrator);
        await SeedFilmsAsync(context, adminUser);
    }

    private static async Task SeedFilmsAsync(AppDbContext context, User admin)
    {
        var movies = new List<Film>
        {
            new()
            {
                Title = "Inception",
                ReleaseDate = new DateOnly(2010, 7, 16),
                Director = new DirectorInfo { Name = "Christopher", Surname = "Nolan" },
                Producer = new ProducerInfo { Name = "Emma Thomas" },
                Actors =
                [
                    new Actor { Name = "Leonardo", Surname = "DiCaprio" },
                    new Actor { Name = "Joseph", Surname = "Gordon-Levitt" },
                    new Actor { Name = "Ellen", Surname = "Page" }
                ],
                Genres =
                [
                    new Genre { Name = "Science Fiction" },
                    new Genre { Name = "Thriller" }
                ],
                Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a CEO.",
                CreatorId = admin.Id.ToString()
            },
            new()
            {
                Title = "The Matrix",
                ReleaseDate = new DateOnly(1999, 3, 31),
                Director = new DirectorInfo { Name = "Lana", Surname = "Wachowski" },
                Producer = new ProducerInfo { Name = "Joel Silver" },
                Actors =
                [
                    new Actor { Name = "Keanu", Surname = "Reeves" },
                    new Actor { Name = "Laurence", Surname = "Fishburne" },
                    new Actor { Name = "Carrie-Anne", Surname = "Moss" }
                ],
                Genres =
                [
                    new Genre { Name = "Action" },
                    new Genre { Name = "Science Fiction" }
                ],
                Description = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                CreatorId = admin.Id.ToString()
            },
            new()
            {
                Title = "The Shawshank Redemption",
                ReleaseDate = new DateOnly(1994, 9, 23),
                Director = new DirectorInfo { Name = "Frank", Surname = "Darabont" },
                Producer = new ProducerInfo { Name = "Niki Marvin" },
                Actors =
                [
                    new Actor { Name = "Tim", Surname = "Robbins" },
                    new Actor { Name = "Morgan", Surname = "Freeman" }
                ],
                Genres =
                [
                    new Genre { Name = "Drama" }
                ],
                Description = "Two imprisoned men bond over a number of years, finding solace and eventual redemption through acts of common decency.",
                CreatorId = admin.Id.ToString()
            },
            new()
            {
                Title = "The Dark Knight",
                ReleaseDate = new DateOnly(2008, 7, 18),
                Director = new DirectorInfo { Name = "Christopher", Surname = "Nolan" },
                Producer = new ProducerInfo { Name = "Emma Thomas" },
                Actors =
                [
                    new Actor { Name = "Christian", Surname = "Bale" },
                    new Actor { Name = "Heath", Surname = "Ledger" },
                    new Actor { Name = "Aaron", Surname = "Eckhart" }
                ],
                Genres =
                [
                    new Genre { Name = "Action" },
                    new Genre { Name = "Crime" },
                    new Genre { Name = "Drama" }
                ],
                Description = "When the menace known as the Joker wreaks havoc and chaos on the people of Gotham, Batman must accept one of the greatest psychological and physical tests of his ability to fight injustice.",
                CreatorId = admin.Id.ToString()
            }
        };

        await context.Films.AddRangeAsync(movies);
        await context.SaveChangesAsync();
    }

    private static async Task<List<User>> SeedUsersAsync(AppDbContext context)
    {
        var users = new List<User>
        {
            new()
            {
                Name = "admin",
                Email = "admin@example.com",
                PasswordHash = PasswordUtils.HashPassword("test"),
                Role = UserRoles.Administrator
            },
            new()
            {
                Name = "moderator",
                Email = "moderator@example.com",
                PasswordHash = PasswordUtils.HashPassword("test"),
                Role = UserRoles.Moderator
            },
            new()
            {
                Name = "user",
                Email = "user@example.com",
                PasswordHash = PasswordUtils.HashPassword("test_user"),
                Role = UserRoles.User
            }
        };

        await context.Users.AddRangeAsync(users);
        await context.SaveChangesAsync();

        return users;
    }
}