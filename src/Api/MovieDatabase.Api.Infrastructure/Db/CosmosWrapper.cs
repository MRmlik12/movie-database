using Microsoft.Azure.Cosmos;

namespace MovieDatabase.Api.Infrastructure.Db;

public class CosmosWrapper(CosmosClient cosmosClient)
{
    internal Database Movies { get; } = cosmosClient.GetDatabase(nameof(Movies));

    internal async Task InitializeContainers()
    {
        await cosmosClient.CreateDatabaseIfNotExistsAsync("Movies");
        await Movies.CreateContainerIfNotExistsAsync(
            id: "Film",
            partitionKeyPath: "/releaseYear",
            throughput: 400
        );
    }
}