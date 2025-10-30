using MovieDatabase.IntegrationTests.Fixtures;
using MovieDatabase.IntegrationTests.Helpers;
using MovieDatabase.IntegrationTests.Responses.Directors;

using Shouldly;

namespace MovieDatabase.IntegrationTests.Queries;

[Collection("AspireAppHost")]
public class DirectorQueryTests(AspireAppHostFixture fixture) : IClassFixture<AspireAppHostFixture>
{
    private readonly HttpClient _httpClient = fixture.CreateHttpClient("movies-db-api");

    [Fact]
    public async Task GetDirectors_WithoutFilter_ShouldReturnAllDirectors()
    {
        const string query = """
                                 query {
                                     directors {
                                         id
                                         name
                                         surname
                                     }
                                 }
                             """;

        var response = await GraphQLHelper.ExecuteQueryAsync<DirectorsResponse>(_httpClient, query);
        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
        response.Data.Directors.ShouldNotBeNull();
        Assert.NotNull(response.Data.Directors);
    }

    [Fact]
    public async Task GetDirectors_WithTermFilter_ShouldReturnFilteredDirectors()
    {
        const string query = """
                                 query GetDirectorsByTerm($term: String) {
                                     directors(term: $term) {
                                         id
                                         name
                                         surname
                                     }
                                 }
                             """;

        var variables = new { term = "Spielberg" };

        var response = await GraphQLHelper.ExecuteQueryAsync<DirectorsResponse>(_httpClient, query, variables);
        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
        Assert.NotNull(response.Data);
    }
}