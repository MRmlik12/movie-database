using Microsoft.Azure.Cosmos;

using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Core.Documents.Users;
using MovieDatabase.Api.Core.Utils;

using User = MovieDatabase.Api.Core.Documents.Users.User;

namespace MovieDatabase.Api.Infrastructure.Db;

public static class CosmosSeeder
{
    public static async Task SeedFilms(Database db, User admin)
    {
        var filmContainer = db.GetContainer(nameof(Film));

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
            }
        };

        foreach (var movie in movies)
        {
            await filmContainer.CreateItemAsync(movie);
        }
    }

    public static async Task<List<User>> SeedUsers(Database db)
    {
        var userContainer = db.GetContainer(nameof(User));

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
                Name = "user",
                Email = "user@example.com",
                PasswordHash = PasswordUtils.HashPassword("test_user"),
                Role = UserRoles.User
            }
        };

        foreach (var user in users)
        {
            await userContainer.UpsertItemAsync(user);
        }

        return users;
    }
}