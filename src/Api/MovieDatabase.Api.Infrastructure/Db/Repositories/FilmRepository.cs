using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

using MovieDatabase.Api.Core.Documents.Films;

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

    public async Task<IEnumerable<Actor>> GetActors(string? searchTerm)
    {
        var query = Container.GetItemLinqQueryable<Film>()
            .SelectMany(f => f.Actors);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var split = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (split.Length == 2)
            {
                query = query.Where(a => (a.Name.Contains(split[0]) && a.Surname.Contains(split[1])) ||
                                         (a.Name.Contains(split[1]) && a.Surname.Contains(split[0])));
            }
            else
            {
                query = query.Where(a => a.Name.Contains(searchTerm) ||
                                         a.Surname.Contains(searchTerm));
            }
        }

        using var iterator = query.ToFeedIterator();

        var results = new List<Actor>();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();

            results.AddRange(response.Resource);
        }

        return results;
    }

    public async Task<IEnumerable<Genre>> GetGenres(string? searchTerm)
    {
        var query = Container.GetItemLinqQueryable<Film>()
            .SelectMany(f => f.Genres);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(g => g.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        using var iterator = query.ToFeedIterator();

        var results = new List<Genre>();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();

            results.AddRange(response.Resource);
        }

        return results;
    }

    public async Task<IEnumerable<DirectorInfo>> GetDirectors(string? searchTerm)
    {
        var query = Container.GetItemLinqQueryable<Film>()
            .Select(f => f.Director);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            var split = searchTerm.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            if (split.Length == 2)
            {
                query = query.Where(a => (a.Name.Contains(split[0]) && a.Surname.Contains(split[1])) ||
                                         (a.Name.Contains(split[1]) && a.Surname.Contains(split[0])));
            }
            else
            {
                query = query.Where(a => a.Name.Contains(searchTerm) ||
                                         a.Surname.Contains(searchTerm));
            }
        }

        using var iterator = query.ToFeedIterator();

        var results = new List<DirectorInfo>();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();

            results.AddRange(response.Resource);
        }

        return results;
    }

    public async Task<IEnumerable<ProducerInfo>> GetProducers(string? searchTerm)
    {
        var query = Container.GetItemLinqQueryable<Film>()
            .Select(f => f.Producer);

        if (!string.IsNullOrEmpty(searchTerm))
        {
            query = query.Where(g => g.Name.Contains(searchTerm, StringComparison.OrdinalIgnoreCase));
        }

        using var iterator = query.ToFeedIterator();

        var results = new List<ProducerInfo>();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();

            results.AddRange(response.Resource);
        }

        return results;
    }

    public async Task<IEnumerable<Film>> GetAll(string? title)
    {
        var query = Container.GetItemLinqQueryable<Film>();

        if (!string.IsNullOrEmpty(title))
        {
            query = (IOrderedQueryable<Film>)query.Where(f => f.Title.StartsWith(title));
        }

        using var iterator = query.ToFeedIterator();

        var results = new List<Film>();
        while (iterator.HasMoreResults)
        {
            var response = await iterator.ReadNextAsync();

            results.AddRange(response.Resource);
        }

        return results;
    }
}