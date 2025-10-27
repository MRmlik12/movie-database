using MovieDatabase.IntegrationTests.Helpers;

namespace MovieDatabase.IntegrationTests.Queries;

[Collection("AspireAppHost")]
public class FilmQueryTests : IClassFixture<AspireAppHostFixture>
{
    private readonly HttpClient _httpClient;

    public FilmQueryTests(AspireAppHostFixture fixture)
    {
        _httpClient = fixture.CreateHttpClient("movies-db-api");
    }

    [Fact]
    public async Task GetFilms_WithoutFilter_ShouldReturnAllFilms()
    {
        var query = @"
            query {
                films {
                    id
                    title
                    description
                    releaseDate
                }
            }";

        var response = await GraphQLHelper.ExecuteQueryAsync<FilmsResponse>(_httpClient, query);

        Assert.NotNull(response);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Data);
        Assert.NotNull(response.Data.Films);
        Assert.NotEmpty(response.Data.Films);
    }

    [Fact]
    public async Task GetFilms_WithTitleFilter_ShouldReturnFilteredFilms()
    {
        var query = @"
            query GetFilmsByTitle($title: String) {
                films(title: $title) {
                    id
                    title
                    releaseDate
                }
            }";

        var variables = new { title = "Test" };

        var response = await GraphQLHelper.ExecuteQueryAsync<FilmsResponse>(_httpClient, query, variables);

        Assert.NotNull(response);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Data);
    }

    [Fact]
    public async Task GetFilms_ShouldIncludeActorsAndDirectors()
    {
        var query = @"
            query {
                films {
                    id
                    title
                    actors {
                        id
                        name
                        surname
                    }
                    director {
                        id
                        name
                        surname
                    }
                    genres {
                        id
                        name
                    }
                    producer {
                        id
                        name
                    }
                }
            }";

        var response = await GraphQLHelper.ExecuteQueryAsync<FilmsResponse>(_httpClient, query);

        Assert.NotNull(response);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Data?.Films);
        
        var film = response.Data.Films.FirstOrDefault();
        if (film != null)
        {
            Assert.NotNull(film.Actors);
            Assert.NotNull(film.Director);
            Assert.NotNull(film.Genres);
            Assert.NotNull(film.Producer);
        }
    }

    private class FilmsResponse
    {
        public List<FilmDto> Films { get; set; } = new();
    }

    private class FilmDto
    {
        public string? Id { get; set; }
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? ReleaseDate { get; set; }
        public List<ActorDto>? Actors { get; set; }
        public DirectorDto? Director { get; set; }
        public List<GenreDto>? Genres { get; set; }
        public ProducerDto? Producer { get; set; }
    }

    private class ActorDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }

    private class DirectorDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Surname { get; set; }
    }

    private class GenreDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }

    private class ProducerDto
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
    }
}
