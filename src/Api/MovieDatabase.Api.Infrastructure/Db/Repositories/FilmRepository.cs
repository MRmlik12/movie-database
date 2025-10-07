using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;
using MovieDatabase.Api.Core.Documents;

namespace MovieDatabase.Api.Infrastructure.Db.Repositories;

public class FilmRepository(CosmosWrapper wrapper) : IFilmRepository
{
    private Container Container { get; } = wrapper.Movies.GetContainer(nameof(Film));

    public async Task Add(Film film)
    {
        await Container.UpsertItemAsync(film);
    }

    public async Task<Film?> GetByName(string name)
    {
        var response = await Container.ReadItemAsync<Film>(name, new PartitionKey(name));

        return response?.Resource;
    }

    public async Task<Film?> GetById(string id)
    {
        var response = await Container.ReadItemAsync<Film>(id, new PartitionKey(id));

        return response?.Resource;
    }

    public async Task<IEnumerable<Film>> GetAll(string title = "")
    {
        using var iterator = Container.GetItemLinqQueryable<Film>(true)
            .Where(f => f.Title.StartsWith(title))
            .ToFeedIterator();

        var results = new List<Film>();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();

            results.AddRange(response.Resource);
        }

        return results;
    }
}
