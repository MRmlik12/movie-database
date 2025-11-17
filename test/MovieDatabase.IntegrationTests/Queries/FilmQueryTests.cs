using MovieDatabase.IntegrationTests.Fixtures;
using MovieDatabase.IntegrationTests.Helpers;
using MovieDatabase.IntegrationTests.Responses.Films;

using Shouldly;

namespace MovieDatabase.IntegrationTests.Queries;

[Collection("AspireAppHost")]
public class FilmQueryTests(AspireAppHostFixture fixture)
{
    private readonly HttpClient _httpClient = fixture.CreateHttpClient("movies-db-api");

    [Fact]
    public async Task GetFilms_WithoutFilter_ShouldReturnAllFilms()
    {
        // Arrange
        const string query = """
                                 query {
                                     films(first: 10) {
                                         nodes {
                                             id
                                             title
                                             description
                                             releaseDate
                                         }
                                     }
                                 }
                             """;

        // Act
        var response = await GraphQLHelper.ExecuteQueryAsync<FilmsResponse>(_httpClient, query);

        // Assert
        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
        response.Data.Films.ShouldNotBeNull();
        response.Data.Films.Nodes.ShouldNotBeNull();
        response.Data.Films.Nodes.ShouldNotBeEmpty();
    }

    [Fact]
    public async Task GetFilms_ShouldIncludeActorsAndDirectors()
    {
        // Arrange
        const string query = """
                                 query {
                                     films(first: 10) {
                                         nodes {
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
                                 }
                             """;

        // Act
        var response = await GraphQLHelper.ExecuteQueryAsync<FilmsResponse>(_httpClient, query);

        // Assert
        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
        response.Data.Films.ShouldNotBeNull();
        response.Data.Films.Nodes.ShouldNotBeNull();

        var film = response.Data.Films.Nodes.FirstOrDefault();
        if (film != null)
        {
            film.Actors.ShouldNotBeNull();
            film.Director.ShouldNotBeNull();
            film.Genres.ShouldNotBeNull();
            film.Producer.ShouldNotBeNull();
        }
    }
}