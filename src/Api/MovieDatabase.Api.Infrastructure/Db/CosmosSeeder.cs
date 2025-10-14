using Microsoft.Azure.Cosmos;

using MovieDatabase.Api.Core.Documents;

namespace MovieDatabase.Api.Infrastructure.Db;

public static class CosmosSeeder
{
    public static async Task SeedFilms(Database db)
    {
        var filmContainer = db.GetContainer(nameof(Film));
        
        var movies = new List<Film>
        {
            new()
            {
                Title = "Inception",
                ReleaseDate = new DateOnly(2010, 7, 16),
                DirectorId = Guid.NewGuid().ToString(),
                ProducerId = Guid.NewGuid().ToString(),
                Director = new DirectorInfo(null, "Christopher", "Nolan"),
                Producer = new ProducerInfo(null, "Emma Thomas"),
                Actors =
                [
                    new Actor(null, "Leonardo", "DiCaprio"),
                    new Actor(null, "Joseph", "Gordon-Levitt"),
                    new Actor(null, "Ellen", "Page")
                ],
                Genres =
                [
                    new Genre(null, "Science Fiction"),
                    new Genre(null, "Thriller")
                ],
                Description = "A thief who steals corporate secrets through the use of dream-sharing technology is given the inverse task of planting an idea into the mind of a CEO."
            },
            new()
            {
                Title = "The Matrix",
                ReleaseDate = new DateOnly(1999, 3, 31),
                DirectorId = Guid.NewGuid().ToString(),
                ProducerId = Guid.NewGuid().ToString(),
                Director = new DirectorInfo(null, "Lana", "Wachowski"),
                Producer = new ProducerInfo(null, "Joel Silver"),
                Actors =
                [
                    new Actor(null, "Keanu", "Reeves"),
                    new Actor(null, "Laurence", "Fishburne"),
                    new Actor(null, "Carrie-Anne", "Moss")
                ],
                Genres =
                [
                    new Genre(null, "Action"),
                    new Genre(null, "Science Fiction")
                ],
                Description = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers."
            }
        };
    
        foreach (var movie in movies)
        {
            await filmContainer.CreateItemAsync(movie);
        }
    }
}