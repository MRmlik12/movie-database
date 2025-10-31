using System.Net.Http.Headers;

using MovieDatabase.Api.Application.Films.CreateFilm;
using MovieDatabase.Api.Application.Films.EditFilm;
using MovieDatabase.Api.Application.Users.AuthenticateUser;
using MovieDatabase.IntegrationTests.Fixtures;
using MovieDatabase.IntegrationTests.Helpers;
using MovieDatabase.IntegrationTests.Responses.Films;
using MovieDatabase.IntegrationTests.Responses.Users;

using Shouldly;

namespace MovieDatabase.IntegrationTests.Mutations;

[Collection("AspireAppHost")]
public class FilmMutationTests(AspireAppHostFixture fixture) : IClassFixture<AspireAppHostFixture>
{
    private readonly HttpClient _httpClient = fixture.CreateHttpClient("movies-db-api");

    [Fact]
    public async Task CreateFilm_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        const string mutation = """
                                    mutation CreateFilm($input: CreateFilmInput!) {
                                        createFilm(input: $input) {
                                            id
                                            title
                                        }
                                    }
                                """;

        var input = new CreateFilmInput(
            Title: "Test Movie",
            ReleaseDate: new DateOnly(2025, 1, 1),
            Description: "Test description",
            Actors: [new CreateFilmInput.ActorPlaceholder(null, "John", "Doe")],
            Genres: [new CreateFilmInput.GenrePlaceholder(null, "Action")],
            Director: new CreateFilmInput.DirectorPlaceholder(null, "Jane", "Smith"),
            Producer: new CreateFilmInput.ProducerPlaceholder(null, "ABC Studios")
        );

        var variables = new { input };

        var response = await GraphQLHelper.ExecuteMutationAsync(_httpClient, mutation, variables);

        var content = await response.Content.ReadAsStringAsync();
        var hasAuthError = content.Contains("authorize", StringComparison.OrdinalIgnoreCase) ||
                          content.Contains("authenticated", StringComparison.OrdinalIgnoreCase) ||
                          content.Contains("unauthorized", StringComparison.OrdinalIgnoreCase) ||
                          response.StatusCode == System.Net.HttpStatusCode.Unauthorized;
        hasAuthError.ShouldBeTrue($"Expected authorization error but got: {content}");
    }

    [Fact(Skip = "Authorization headers are not properly propagated to GraphQL requests in the current test infrastructure")]
    public async Task CreateFilm_WithAdminUser_ShouldCreateFilm()
    {
        var token = await GetAdminTokenAsync();

        var client = fixture.CreateHttpClient("movies-db-api");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        const string mutation = """
                                    mutation CreateFilm($input: CreateFilmInput!) {
                                        createFilm(input: $input) {
                                            id
                                            title
                                            releaseDate
                                            description
                                        }
                                    }
                                """;

        var input = new CreateFilmInput(
            Title: $"Integration Test Movie {Guid.NewGuid():N}",
            ReleaseDate: new DateOnly(2025, 6, 15),
            Description: "Created during integration test",
            Actors: [new CreateFilmInput.ActorPlaceholder(null, "Test", "Actor")],
            Genres: [new CreateFilmInput.GenrePlaceholder(null, "Drama")],
            Director: new CreateFilmInput.DirectorPlaceholder(null, "Test", "Director"),
            Producer: new CreateFilmInput.ProducerPlaceholder(null, "Test Studios")
        );

        var variables = new { input };

        var response = await GraphQLHelper.ExecuteMutationAsync<CreateFilmResponse>(
            client, mutation, variables);

        if (response?.Errors != null && response.Errors.Length > 0)
        {
            var errorMsg = string.Join(", ", response.Errors.Select(e => e.Message));
            throw new Exception($"GraphQL mutation failed with errors: {errorMsg}. Token was: {token[..20]}...");
        }

        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
        response.Data.CreateFilm.ShouldNotBeNull();
        response.Data.CreateFilm.Id.ShouldNotBeNull();
        response.Data.CreateFilm.Title.ShouldNotBeNull();
        response.Data.CreateFilm.Title.ShouldContain("Integration Test Movie");
    }

    [Fact(Skip = "Authorization headers are not properly propagated to GraphQL requests in the current test infrastructure")]
    public async Task EditFilm_WithModeratorUser_ShouldUpdateFilm()
    {
        var adminToken = await GetAdminTokenAsync();

        var client = fixture.CreateHttpClient("movies-db-api");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

        const string createMutation = """
                                          mutation CreateFilm($input: CreateFilmInput!) {
                                              createFilm(input: $input) {
                                                  id
                                                  title
                                              }
                                          }
                                      """;

        var createInput = new CreateFilmInput(
            Title: $"Film to Edit {Guid.NewGuid():N}",
            ReleaseDate: new DateOnly(2025, 1, 1),
            Description: "Original description",
            Actors: [new CreateFilmInput.ActorPlaceholder(null, "Actor", "One")],
            Genres: [new CreateFilmInput.GenrePlaceholder(null, "Comedy")],
            Director: new CreateFilmInput.DirectorPlaceholder(null, "Director", "One"),
            Producer: new CreateFilmInput.ProducerPlaceholder(null, "Producer One")
        );

        var createResponse = await GraphQLHelper.ExecuteMutationAsync<CreateFilmResponse>(
            client, createMutation, new { input = createInput });

        if (createResponse?.Errors != null && createResponse.Errors.Length > 0)
        {
            var errorMsg = string.Join(", ", createResponse.Errors.Select(e => e.Message));
            throw new Exception($"Failed to create film for editing test: {errorMsg}");
        }

        createResponse.ShouldNotBeNull();
        createResponse.Data.ShouldNotBeNull();
        createResponse.Data.CreateFilm.ShouldNotBeNull();
        createResponse.Data.CreateFilm.Id.ShouldNotBeNull();
        var filmId = createResponse.Data.CreateFilm.Id;

        const string editMutation = """
                                        mutation EditFilm($input: EditFilmInput!) {
                                            editFilm(input: $input) {
                                                id
                                                title
                                                description
                                            }
                                        }
                                    """;

        var editInput = new EditFilmInput(
            Id: filmId,
            Title: "Updated Title",
            ReleaseDate: new DateOnly(2025, 2, 15),
            Description: "Updated description",
            Actors: [new EditFilmInput.EditFilmActorPlaceholder(null, "Updated", "Actor")],
            Genres: [new EditFilmInput.EditFilmGenrePlaceholder(null, "Drama")],
            Director: new EditFilmInput.EditFilmDirectorPlaceholder(null, "Updated", "Director"),
            Producer: new EditFilmInput.EditFilmProducerPlaceholder(null, "Updated Producer")
        );

        var editResponse = await GraphQLHelper.ExecuteMutationAsync<EditFilmResponse>(
            client, editMutation, new { input = editInput });

        editResponse.ShouldNotBeNull();
        editResponse.Errors.ShouldBeNull();
        editResponse.Data.ShouldNotBeNull();
        editResponse.Data.EditFilm.ShouldNotBeNull();
        editResponse.Data.EditFilm.Title.ShouldBe("Updated Title");
        editResponse.Data.EditFilm.Description.ShouldBe("Updated description");
    }

    [Fact(Skip = "Authorization headers are not properly propagated to GraphQL requests in the current test infrastructure")]
    public async Task DeleteFilm_WithAdminUser_ShouldDeleteFilm()
    {
        var token = await GetAdminTokenAsync();

        var client = fixture.CreateHttpClient("movies-db-api");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        const string createMutation = """
                                          mutation CreateFilm($input: CreateFilmInput!) {
                                              createFilm(input: $input) {
                                                  id
                                                  title
                                              }
                                          }
                                      """;

        var createInput = new CreateFilmInput(
            Title: $"Film to Delete {Guid.NewGuid():N}",
            ReleaseDate: new DateOnly(2025, 1, 1),
            Description: "Will be deleted",
            Actors: [new CreateFilmInput.ActorPlaceholder(null, "Actor", "Name")],
            Genres: [new CreateFilmInput.GenrePlaceholder(null, "Thriller")],
            Director: new CreateFilmInput.DirectorPlaceholder(null, "Director", "Name"),
            Producer: new CreateFilmInput.ProducerPlaceholder(null, "Producer Name")
        );

        var createResponse = await GraphQLHelper.ExecuteMutationAsync<CreateFilmResponse>(
            client, createMutation, new { input = createInput });

        if (createResponse?.Errors != null && createResponse.Errors.Length > 0)
        {
            var errorMsg = string.Join(", ", createResponse.Errors.Select(e => e.Message));
            throw new Exception($"Failed to create film for deletion test: {errorMsg}");
        }

        createResponse.ShouldNotBeNull();
        createResponse.Data.ShouldNotBeNull();
        createResponse.Data.CreateFilm.ShouldNotBeNull();
        createResponse.Data.CreateFilm.Id.ShouldNotBeNull();
        var filmId = createResponse.Data.CreateFilm.Id;

        const string deleteMutation = """
                                          mutation DeleteFilm($filmId: String!) {
                                              deleteFilm(filmId: $filmId)
                                          }
                                      """;

        var deleteVariables = new { filmId };

        var deleteResponse = await GraphQLHelper.ExecuteMutationAsync<DeleteFilmResponse>(
            client, deleteMutation, deleteVariables);

        deleteResponse.ShouldNotBeNull();
        deleteResponse.Errors.ShouldBeNull();
        deleteResponse.Data.ShouldNotBeNull();
        deleteResponse.Data.DeleteFilm.ShouldNotBeNull();
    }

    private async Task<string> GetAdminTokenAsync()
    {
        const string loginMutation = """
                                         mutation LoginUser($request: AuthenticateUserRequestInput!) {
                                             loginUser(request: $request) {
                                                 token
                                                 id
                                                 username
                                                 email
                                                 role
                                             }
                                         }
                                     """;

        var request = new AuthenticateUserRequest(
            Email: "admin@example.com",
            Password: "test"
        );

        var loginResponse = await GraphQLHelper.ExecuteMutationAsync<LoginUserResponse>(
            _httpClient, loginMutation, new { request });

        if (loginResponse?.Data?.LoginUser?.Token != null)
        {
            return loginResponse.Data.LoginUser.Token;
        }

        var error = loginResponse?.Errors?.FirstOrDefault();
        throw new Exception($"Could not get admin token. Error: {error?.Message ?? "Unknown error"}. Check if seeded admin exists with email 'admin@example.com' and password 'test'");

    }
}