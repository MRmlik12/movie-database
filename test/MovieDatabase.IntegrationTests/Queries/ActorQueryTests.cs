using MovieDatabase.IntegrationTests.Helpers;

namespace MovieDatabase.IntegrationTests.Queries;

[Collection("AspireAppHost")]
public class ActorQueryTests : IClassFixture<AspireAppHostFixture>
{
    private readonly HttpClient _httpClient;

    public ActorQueryTests(AspireAppHostFixture fixture)
    {
        _httpClient = fixture.CreateHttpClient("movies-db-api");
    }

    [Fact]
    public async Task GetActors_WithoutFilter_ShouldReturnAllActors()
    {
        var query = @"
            query {
                actors {
                    id
                    name
                    surname
                }
            }";

        var response = await GraphQLHelper.ExecuteQueryAsync<ActorsResponse>(_httpClient, query);

        Assert.NotNull(response);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Actors);
    }

    [Fact]
    public async Task GetActors_WithTermFilter_ShouldReturnFilteredActors()
    {
        var query = @"
            query GetActorsByTerm($term: String) {
                actors(term: $term) {
                    id
                    name
                    surname
                }
            }";

        var variables = new { term = "Smith" };

        var response = await GraphQLHelper.ExecuteQueryAsync<ActorsResponse>(_httpClient, query, variables);

        Assert.NotNull(response);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Data);
    }

    private class ActorsResponse
    {
        public List<ActorDto> Actors { get; set; } = new();
    }

    private class ActorDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }
}

