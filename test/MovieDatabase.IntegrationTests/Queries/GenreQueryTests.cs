using MovieDatabase.IntegrationTests.Helpers;

namespace MovieDatabase.IntegrationTests.Queries;

[Collection("AspireAppHost")]
public class GenreQueryTests : IClassFixture<AspireAppHostFixture>
{
    private readonly HttpClient _httpClient;

    public GenreQueryTests(AspireAppHostFixture fixture)
    {
        _httpClient = fixture.CreateHttpClient("movies-db-api");
    }

    [Fact]
    public async Task GetGenres_WithoutFilter_ShouldReturnAllGenres()
    {
        var query = @"
            query {
                genres {
                    id
                    name
                }
            }";

        var response = await GraphQLHelper.ExecuteQueryAsync<GenresResponse>(_httpClient, query);

        Assert.NotNull(response);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Genres);
    }

    [Fact]
    public async Task GetGenres_WithTermFilter_ShouldReturnFilteredGenres()
    {
        var query = @"
            query GetGenresByTerm($term: String) {
                genres(term: $term) {
                    id
                    name
                }
            }";

        var variables = new { term = "Action" };

        var response = await GraphQLHelper.ExecuteQueryAsync<GenresResponse>(_httpClient, query, variables);

        Assert.NotNull(response);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Data);
    }

    private class GenresResponse
    {
        public List<GenreDto> Genres { get; set; } = new();
    }

    private class GenreDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}

