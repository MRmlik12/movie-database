using MovieDatabase.IntegrationTests.Fixtures;
using MovieDatabase.IntegrationTests.Helpers;
using MovieDatabase.IntegrationTests.Responses.Actors;

using Shouldly;

namespace MovieDatabase.IntegrationTests.Queries;

[Collection("AspireAppHost")]
public class ActorQueryTests(AspireAppHostFixture fixture)
{
    private readonly HttpClient _httpClient = fixture.CreateHttpClient("movies-db-api");

    [Fact]
    public async Task GetActors_WithoutFilter_ShouldReturnAllActors()
    {
        const string query = """
                                 query {
                                     actors {
                                         id
                                         name
                                         surname
                                     }
                                 }
                             """;

        var response = await GraphQLHelper.ExecuteQueryAsync<ActorsResponse>(_httpClient, query);

        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
        response.Data.Actors.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetActors_WithTermFilter_ShouldReturnFilteredActors()
    {
        const string query = """
                                 query GetActorsByTerm($term: String) {
                                     actors(term: $term) {
                                         id
                                         name
                                         surname
                                     }
                                 }
                             """;

        var variables = new { term = "Smith" };

        var response = await GraphQLHelper.ExecuteQueryAsync<ActorsResponse>(_httpClient, query, variables);

        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
    }
}