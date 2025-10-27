using MovieDatabase.IntegrationTests.Helpers;

namespace MovieDatabase.IntegrationTests.Queries;

[Collection("AspireAppHost")]
public class ProducerQueryTests : IClassFixture<AspireAppHostFixture>
{
    private readonly HttpClient _httpClient;

    public ProducerQueryTests(AspireAppHostFixture fixture)
    {
        _httpClient = fixture.CreateHttpClient("movies-db-api");
    }

    [Fact]
    public async Task GetProducers_WithoutFilter_ShouldReturnAllProducers()
    {
        var query = @"
            query {
                producers {
                    id
                    name
                }
            }";

        var response = await GraphQLHelper.ExecuteQueryAsync<ProducersResponse>(_httpClient, query);

        Assert.NotNull(response);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Producers);
    }

    [Fact]
    public async Task GetProducers_WithTermFilter_ShouldReturnFilteredProducers()
    {
        var query = @"
            query GetProducersByTerm($term: String) {
                producers(term: $term) {
                    id
                    name
                }
            }";

        var variables = new { term = "Warner" };

        var response = await GraphQLHelper.ExecuteQueryAsync<ProducersResponse>(_httpClient, query, variables);

        Assert.NotNull(response);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Data);
    }

    private class ProducersResponse
    {
        public List<ProducerDto> Producers { get; set; } = new();
    }

    private class ProducerDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}

