using Microsoft.Azure.Cosmos;

namespace MovieDatabase.Api.Infrastructure.Db;

public class CosmosWrapper(CosmosClient cosmosClient)
{
    internal Database Movies { get; } = cosmosClient.GetDatabase("Movies");
}