using Microsoft.Azure.Cosmos;
using Microsoft.Azure.Cosmos.Linq;

using User = MovieDatabase.Api.Core.Documents.Users.User;

namespace MovieDatabase.Api.Infrastructure.Db.Repositories;

public class UserRepository(CosmosWrapper wrapper) : IUserRepository
{
    private Container Container { get; } = wrapper.Movies.GetContainer(nameof(User));
    
    public async Task Add(User user)
    {
        await Container.UpsertItemAsync(user);
    }
    
    public async Task<User?> GetByEmail(string email)
    {
        using var iter = Container.GetItemLinqQueryable<User>()
            .Where(u => u.Email == email)
            .ToFeedIterator();

        while (iter.HasMoreResults)
        {
            var response = await iter.ReadNextAsync();
            
            return response.SingleOrDefault();
        }

        return null;
    }
}