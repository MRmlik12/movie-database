using Microsoft.Azure.Cosmos;

using MovieDatabase.Api.Core.Documents;

namespace MovieDatabase.Api.Infrastructure.Db.Repositories;

public class GenreRepository(CosmosWrapper wrapper) : IGenreRepository
{
    private Container Container { get; } = wrapper.Movies.GetContainer(nameof(Genre));
    
    public async Task<Genre?> GetById(string id)
    {
        var response = await Container.ReadItemAsync<Genre>(id, new PartitionKey(id));

        return response?.Resource;
    }

    public async Task<Genre?> GetByName(string name)
    {
        var response = await Container.ReadItemAsync<Genre>(name, new PartitionKey(name));

        return response?.Resource;
    }
}