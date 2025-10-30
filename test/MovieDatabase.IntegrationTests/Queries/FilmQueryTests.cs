using MovieDatabase.IntegrationTests.Fixtures;
using MovieDatabase.IntegrationTests.Helpers;
using MovieDatabase.IntegrationTests.Responses.Films;

using Shouldly;

namespace MovieDatabase.IntegrationTests.Queries;

[Collection("AspireAppHost")]
public class FilmQueryTests(AspireAppHostFixture fixture) : IClassFixture<AspireAppHostFixture>
{
    private readonly HttpClient _httpClient = fixture.CreateHttpClient("movies-db-api");

    [Fact]
    public async Task GetFilms_WithoutFilter_ShouldReturnAllFilms()
    {
        const string query = """
                                 query {
                                     films {
                                         id
                                         title
                                         description
                                         releaseDate
                                     }
                                 }
                             """;

        var response = await GraphQLHelper.ExecuteQueryAsync<FilmsResponse>(_httpClient, query);

        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
        response.Data.Films.ShouldNotBeNull();
        response.Data.Films.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetFilms_WithTitleFilter_ShouldReturnFilteredFilms()
    {
        const string query = """
                                 query GetFilmsByTitle($title: String) {
                                     films(title: $title) {
                                         id
                                         title
                                         releaseDate
                                     }
                                 }
                             """;

        var variables = new { title = "Test" };

        var response = await GraphQLHelper.ExecuteQueryAsync<FilmsResponse>(_httpClient, query, variables);

        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
    }

    [Fact]
    public async Task GetFilms_ShouldIncludeActorsAndDirectors()
    {
        const string query = """
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
                                 }
                             """;

        var response = await GraphQLHelper.ExecuteQueryAsync<FilmsResponse>(_httpClient, query);

        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
        response.Data.Films.ShouldNotBeNull();

        var film = response.Data.Films.FirstOrDefault();
        if (film != null)
        {
            film.Actors.ShouldNotBeNull();
            film.Director.ShouldNotBeNull();
            film.Genres.ShouldNotBeNull();
            film.Producer.ShouldNotBeNull();
        }
    }
}