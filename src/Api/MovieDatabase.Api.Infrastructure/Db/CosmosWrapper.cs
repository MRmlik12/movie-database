using Microsoft.Azure.Cosmos;

using MovieDatabase.Api.Core.Documents;

namespace MovieDatabase.Api.Infrastructure.Db;

public class CosmosWrapper(CosmosClient cosmosClient)
{
    internal Database Movies { get; } = cosmosClient.GetDatabase(nameof(Movies));
    
    internal async Task InitializeContainers()
    {
        await cosmosClient.CreateDatabaseIfNotExistsAsync("Movies");
        await Movies.CreateContainerIfNotExistsAsync(nameof(Film), Film.PartitionKey);
    }
}