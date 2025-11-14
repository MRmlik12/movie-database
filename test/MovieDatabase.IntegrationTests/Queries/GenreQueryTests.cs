using MovieDatabase.IntegrationTests.Fixtures;
using MovieDatabase.IntegrationTests.Helpers;
using MovieDatabase.IntegrationTests.Responses.Genres;

using Shouldly;

namespace MovieDatabase.IntegrationTests.Queries;

[Collection("AspireAppHost")]
public class GenreQueryTests(AspireAppHostFixture fixture)
{
    private readonly HttpClient _httpClient = fixture.CreateHttpClient("movies-db-api");

    [Fact]
    public async Task GetGenres_WithoutFilter_ShouldReturnAllGenres()
    {
        const string query = """
                                 query {
                                     genres(first: 10) {
                                         nodes {
                                             id
                                             name
                                         }
                                     }
                                 }
                             """;

        var response = await GraphQLHelper.ExecuteQueryAsync<GenresResponse>(_httpClient, query);
        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
        response.Data.Genres.ShouldNotBeNull();
        response.Data.Genres.Nodes.ShouldNotBeNull();
        response.Data.Genres.Nodes.ShouldNotBeEmpty();
    }
}