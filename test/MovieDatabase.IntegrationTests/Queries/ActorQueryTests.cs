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
                                     actors(first: 10) {
                                         nodes {
                                             id
                                             name
                                             surname
                                         }
                                     }
                                 }
                             """;

        var response = await GraphQLHelper.ExecuteQueryAsync<ActorsResponse>(_httpClient, query);

        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
        response.Data.Actors.ShouldNotBeNull();
        response.Data.Actors.Nodes.ShouldNotBeNull();
        response.Data.Actors.Nodes.ShouldNotBeEmpty();
    }
}