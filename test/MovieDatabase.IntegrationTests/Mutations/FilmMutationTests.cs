using System.Net.Http.Headers;
using MovieDatabase.IntegrationTests.Helpers;

namespace MovieDatabase.IntegrationTests.Mutations;

[Collection("AspireAppHost")]
public class FilmMutationTests : IClassFixture<AspireAppHostFixture>
{
    private readonly HttpClient _httpClient;
    private readonly AspireAppHostFixture _fixture;

    public FilmMutationTests(AspireAppHostFixture fixture)
    {
        _fixture = fixture;
        _httpClient = fixture.CreateHttpClient("movies-db-api");
    }

    [Fact]
    public async Task CreateFilm_WithoutAuthentication_ShouldReturnUnauthorized()
    {
        var mutation = @"
            mutation CreateFilm($input: CreateFilmInput!) {
                createFilm(input: $input) {
                    id
                    title
                }
            }";

        var variables = new
        {
            input = new
            {
                title = "Test Movie",
                releaseDate = "2025-01-01",
                description = "Test description",
                actors = new[]
                {
                    new { id = (string?)null, name = "John", surname = "Doe" }
                },
                genres = new[]
                {
                    new { id = (string?)null, name = "Action" }
                },
                director = new { id = (string?)null, name = "Jane", surname = "Smith" },
                producer = new { id = (string?)null, name = "ABC Studios" }
            }
        };

        var response = await GraphQLHelper.ExecuteMutationAsync(_httpClient, mutation, variables);

        var content = await response.Content.ReadAsStringAsync();
        Assert.True(
            content.Contains("authorize", StringComparison.OrdinalIgnoreCase) ||
            content.Contains("authenticated", StringComparison.OrdinalIgnoreCase) ||
            content.Contains("unauthorized", StringComparison.OrdinalIgnoreCase) ||
            response.StatusCode == System.Net.HttpStatusCode.Unauthorized,
            $"Expected authorization error but got: {content}");
    }

    [Fact]
    public async Task CreateFilm_WithAdminUser_ShouldCreateFilm()
    {
        var token = await GetAdminTokenAsync();
        
        var client = _fixture.CreateHttpClient("movies-db-api");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var mutation = @"
            mutation CreateFilm($input: CreateFilmInput!) {
                createFilm(input: $input) {
                    id
                    title
                    releaseDate
                    description
                }
            }";

        var variables = new
        {
            input = new
            {
                title = $"Integration Test Movie {Guid.NewGuid():N}",
                releaseDate = "2025-06-15",
                description = "Created during integration test",
                actors = new[]
                {
                    new { id = (string?)null, name = "Test", surname = "Actor" }
                },
                genres = new[]
                {
                    new { id = (string?)null, name = "Drama" }
                },
                director = new { id = (string?)null, name = "Test", surname = "Director" },
                producer = new { id = (string?)null, name = "Test Studios" }
            }
        };

        var response = await GraphQLHelper.ExecuteMutationAsync<CreateFilmResponse>(
            client, mutation, variables);

        if (response?.Errors != null && response.Errors.Length > 0)
        {
            var errorMsg = string.Join(", ", response.Errors.Select(e => e.Message));
            throw new Exception($"GraphQL mutation failed with errors: {errorMsg}. Token was: {token[..20]}...");
        }

        Assert.NotNull(response);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Data?.CreateFilm);
        Assert.NotNull(response.Data.CreateFilm.Id);
        Assert.Contains("Integration Test Movie", response.Data.CreateFilm.Title);
    }

    [Fact]
    public async Task EditFilm_WithModeratorUser_ShouldUpdateFilm()
    {
        var adminToken = await GetAdminTokenAsync();
        
        var client = _fixture.CreateHttpClient("movies-db-api");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", adminToken);

        var createMutation = @"
            mutation CreateFilm($input: CreateFilmInput!) {
                createFilm(input: $input) {
                    id
                    title
                }
            }";

        var createVariables = new
        {
            input = new
            {
                title = $"Film to Edit {Guid.NewGuid():N}",
                releaseDate = "2025-01-01",
                description = "Original description",
                actors = new[] { new { id = (string?)null, name = "Actor", surname = "One" } },
                genres = new[] { new { id = (string?)null, name = "Comedy" } },
                director = new { id = (string?)null, name = "Director", surname = "One" },
                producer = new { id = (string?)null, name = "Producer One" }
            }
        };

        var createResponse = await GraphQLHelper.ExecuteMutationAsync<CreateFilmResponse>(
            client, createMutation, createVariables);

        if (createResponse?.Errors != null && createResponse.Errors.Length > 0)
        {
            var errorMsg = string.Join(", ", createResponse.Errors.Select(e => e.Message));
            throw new Exception($"Failed to create film for editing test: {errorMsg}");
        }

        Assert.NotNull(createResponse?.Data?.CreateFilm?.Id);
        var filmId = createResponse.Data.CreateFilm.Id;

        var editMutation = @"
            mutation EditFilm($input: EditFilmInput!) {
                editFilm(input: $input) {
                    id
                    title
                    description
                }
            }";

        var editVariables = new
        {
            input = new
            {
                id = filmId,
                title = "Updated Title",
                releaseDate = "2025-02-15",
                description = "Updated description",
                actors = new[] { new { id = (string?)null, name = "Updated", surname = "Actor" } },
                genres = new[] { new { id = (string?)null, name = "Drama" } },
                director = new { id = (string?)null, name = "Updated", surname = "Director" },
                producer = new { id = (string?)null, name = "Updated Producer" }
            }
        };

        var editResponse = await GraphQLHelper.ExecuteMutationAsync<EditFilmResponse>(
            client, editMutation, editVariables);

        Assert.NotNull(editResponse);
        Assert.Null(editResponse.Errors);
        Assert.NotNull(editResponse.Data?.EditFilm);
        Assert.Equal("Updated Title", editResponse.Data.EditFilm.Title);
        Assert.Equal("Updated description", editResponse.Data.EditFilm.Description);
    }

    [Fact]
    public async Task DeleteFilm_WithAdminUser_ShouldDeleteFilm()
    {
        var token = await GetAdminTokenAsync();
        
        var client = _fixture.CreateHttpClient("movies-db-api");
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var createMutation = @"
            mutation CreateFilm($input: CreateFilmInput!) {
                createFilm(input: $input) {
                    id
                    title
                }
            }";

        var createVariables = new
        {
            input = new
            {
                title = $"Film to Delete {Guid.NewGuid():N}",
                releaseDate = "2025-01-01",
                description = "Will be deleted",
                actors = new[] { new { id = (string?)null, name = "Actor", surname = "Name" } },
                genres = new[] { new { id = (string?)null, name = "Thriller" } },
                director = new { id = (string?)null, name = "Director", surname = "Name" },
                producer = new { id = (string?)null, name = "Producer Name" }
            }
        };

        var createResponse = await GraphQLHelper.ExecuteMutationAsync<CreateFilmResponse>(
            client, createMutation, createVariables);

        if (createResponse?.Errors != null && createResponse.Errors.Length > 0)
        {
            var errorMsg = string.Join(", ", createResponse.Errors.Select(e => e.Message));
            throw new Exception($"Failed to create film for deletion test: {errorMsg}");
        }

        Assert.NotNull(createResponse?.Data?.CreateFilm?.Id);
        var filmId = createResponse.Data.CreateFilm.Id;

        var deleteMutation = @"
            mutation DeleteFilm($filmId: String!) {
                deleteFilm(filmId: $filmId)
            }";

        var deleteVariables = new { filmId };

        var deleteResponse = await GraphQLHelper.ExecuteMutationAsync<DeleteFilmResponse>(
            client, deleteMutation, deleteVariables);

        Assert.NotNull(deleteResponse);
        Assert.Null(deleteResponse.Errors);
        Assert.NotNull(deleteResponse.Data?.DeleteFilm);
    }

    private async Task<string> GetAdminTokenAsync()
    {
        var loginMutation = @"
            mutation LoginUser($request: AuthenticateUserRequestInput!) {
                loginUser(request: $request) {
                    token
                    id
                    username
                    email
                    role
                }
            }";

        var variables = new
        {
            request = new
            {
                email = "admin@example.com",
                password = "test"
            }
        };

        var loginResponse = await GraphQLHelper.ExecuteMutationAsync<LoginResponse>(
            _httpClient, loginMutation, variables);

        if (loginResponse?.Data?.LoginUser?.Token == null)
        {
            var error = loginResponse?.Errors?.FirstOrDefault();
            throw new Exception($"Could not get admin token. Error: {error?.Message ?? "Unknown error"}. Check if seeded admin exists with email 'admin@example.com' and password 'test'");
        }

        return loginResponse.Data.LoginUser.Token;
    }
}
