using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

using MovieDatabase.Api.Core.Documents;

namespace MovieDatabase.Api.Infrastructure.Db.Repositories;
 
public class ActorRepository(CosmosWrapper wrapper) : IActorRepository
{
    private Container Container { get; } = wrapper.Movies.GetContainer(nameof(Actor));
    
    public async Task<Actor?> GetByIdAsync(string id)
    {
        var response = await Container.ReadItemAsync<Actor>(id, new PartitionKey(id));

        return response?.Resource;
    }

    public async Task<Actor?> GetByNameSurname(string name, string surname)
    {
        using var iterator = Container.GetItemLinqQueryable<Actor>()
            .Where(a => a.Name.Equals(name, StringComparison.CurrentCultureIgnoreCase) &&
                        a.Surname.Equals(surname, StringComparison.CurrentCultureIgnoreCase))
            .ToFeedIterator();

        var actor = await iterator.ReadNextAsync();

        // TODO: Finish implementation
        return actor.Resource.;
    }
}