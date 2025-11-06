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
                                             producers {
                                                 id
                                                 name
                                             }
                                         }
                             """;

        var response = await GraphQLHelper.ExecuteQueryAsync<ProducersResponse>(_httpClient, query);

        Assert.NotNull(response);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Producers);
    }

    [Fact]
    public async Task GetProducers_WithTermFilter_ShouldReturnFilteredProducers()
    {
        const string query = """
                                 query GetProducersByTerm($term: String) {
                                     producers(term: $term) {
                                         id
                                         name
                                     }
                                 }
                             """;

        var variables = new { term = "Warner" };

        var response = await GraphQLHelper.ExecuteQueryAsync<ProducersResponse>(_httpClient, query, variables);

        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
    }
}