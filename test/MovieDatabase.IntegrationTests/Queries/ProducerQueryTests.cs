using MovieDatabase.IntegrationTests.Fixtures;
using MovieDatabase.IntegrationTests.Helpers;
using MovieDatabase.IntegrationTests.Responses.Producers;

using Shouldly;

namespace MovieDatabase.IntegrationTests.Queries;

[Collection("AspireAppHost")]
public class ProducerQueryTests(AspireAppHostFixture fixture)
{
    private readonly HttpClient _httpClient = fixture.CreateHttpClient("movies-db-api");

    [Fact]
    public async Task GetProducers_WithoutFilter_ShouldReturnAllProducers()
    {
        const string query = """
                                 query {
                                     producers(first: 10) {
                                         nodes {
                                             id
                                             name
                                         }
                                     }
                                 }
                             """;

        var response = await GraphQLHelper.ExecuteQueryAsync<ProducersResponse>(_httpClient, query);

        response.ShouldNotBeNull();
        if (response.Errors != null)
        {
            Console.WriteLine($"GraphQL Errors: {string.Join("; ", response.Errors.Select(e => e.Message))}");
        }
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
        response.Data.Producers.ShouldNotBeNull();
        response.Data.Producers.Nodes.ShouldNotBeNull();
        response.Data.Producers.Nodes.ShouldNotBeEmpty();
    }
}