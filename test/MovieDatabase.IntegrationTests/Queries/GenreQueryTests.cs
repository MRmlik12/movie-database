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
                                     genres {
                                         id
                                         name
                                     }
                                 }
                             """;

        var response = await GraphQLHelper.ExecuteQueryAsync<GenresResponse>(_httpClient, query);
        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
        response.Data.Genres.ShouldNotBeNull();
        Assert.NotNull(response.Data.Genres);
    }

    [Fact]
    public async Task GetGenres_WithTermFilter_ShouldReturnFilteredGenres()
    {
        const string query = """
                                 query GetGenresByTerm($term: String) {
                                     genres(term: $term) {
                                         id
                                         name
                                     }
                                 }
                             """;

        var variables = new { term = "Action" };

        var response = await GraphQLHelper.ExecuteQueryAsync<GenresResponse>(_httpClient, query, variables);

        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
    }
}