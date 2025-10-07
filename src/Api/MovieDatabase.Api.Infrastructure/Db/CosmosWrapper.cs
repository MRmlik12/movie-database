using Microsoft.Azure.Cosmos;

namespace MovieDatabase.Api.Infrastructure.Db;

public class CosmosWrapper(CosmosClient cosmosClient)
{
    internal Database MovieDatabase { get; } = cosmosClient.GetDatabase("Movies");
}