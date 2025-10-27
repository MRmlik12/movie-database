using MovieDatabase.IntegrationTests.Helpers;

namespace MovieDatabase.IntegrationTests.Queries;

[Collection("AspireAppHost")]
public class DirectorQueryTests : IClassFixture<AspireAppHostFixture>
{
    private readonly HttpClient _httpClient;

    public DirectorQueryTests(AspireAppHostFixture fixture)
    {
        _httpClient = fixture.CreateHttpClient("movies-db-api");
    }

    [Fact]
    public async Task GetDirectors_WithoutFilter_ShouldReturnAllDirectors()
    {
        var query = @"
            query {
                directors {
                    id
                    name
                    surname
                }
            }";

        var response = await GraphQLHelper.ExecuteQueryAsync<DirectorsResponse>(_httpClient, query);

        Assert.NotNull(response);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Directors);
    }

    [Fact]
    public async Task GetDirectors_WithTermFilter_ShouldReturnFilteredDirectors()
    {
        var query = @"
            query GetDirectorsByTerm($term: String) {
                directors(term: $term) {
                    id
                    name
                    surname
                }
            }";

        var variables = new { term = "Spielberg" };

        var response = await GraphQLHelper.ExecuteQueryAsync<DirectorsResponse>(_httpClient, query, variables);

        Assert.NotNull(response);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Data);
    }

    private class DirectorsResponse
    {
        public List<DirectorDto> Directors { get; set; } = new();
    }

    private class DirectorDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}
