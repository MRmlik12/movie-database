using Microsoft.Azure.Cosmos;

using MovieDatabase.Api.Core.Documents.Films;

using User = MovieDatabase.Api.Core.Documents.Users.User;

namespace MovieDatabase.Api.Infrastructure.Db;

public class CosmosWrapper(CosmosClient cosmosClient)
{
    internal Database Movies { get; } = cosmosClient.GetDatabase(nameof(Movies));

    internal async Task InitializeContainers()
    {
        await cosmosClient.CreateDatabaseIfNotExistsAsync(nameof(Movies));

        await Movies.CreateContainerIfNotExistsAsync(nameof(Film), Film.PartitionKey);
        await Movies.CreateContainerIfNotExistsAsync(nameof(User), User.PartitionKey);
    }
}