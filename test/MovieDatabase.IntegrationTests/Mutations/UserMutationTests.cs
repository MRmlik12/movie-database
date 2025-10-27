using System.Net.Http.Headers;
using MovieDatabase.IntegrationTests.Helpers;

namespace MovieDatabase.IntegrationTests.Mutations;

[Collection("AspireAppHost")]
public class UserMutationTests : IClassFixture<AspireAppHostFixture>
{
    private readonly HttpClient _httpClient;

    public UserMutationTests(AspireAppHostFixture fixture)
    {
        _httpClient = fixture.CreateHttpClient("movies-db-api");
    }

    [Fact]
    public async Task RegisterUser_WithValidData_ShouldReturnCredentials()
    {
        var mutation = @"
            mutation RegisterUser($request: CreateUserRequestInput!) {
                registerUser(request: $request) {
                    token
                    expireTime
                }
            }";

        var variables = new
        {
            request = new
            {
                username = $"testuser_{Guid.NewGuid():N}",
                email = $"test_{Guid.NewGuid():N}@example.com",
                password = "SecurePassword123!"
            }
        };

        var response = await GraphQLHelper.ExecuteMutationAsync<RegisterUserResponse>(
            _httpClient, mutation, variables);

        Assert.NotNull(response);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Data?.RegisterUser);
        Assert.NotNull(response.Data.RegisterUser.Token);
        Assert.True(response.Data.RegisterUser.ExpireTime > DateTime.UtcNow);
    }

    [Fact]
    public async Task RegisterUser_WithDuplicateEmail_ShouldReturnError()
    {
        var email = $"duplicate_{Guid.NewGuid():N}@example.com";

        var mutation = @"
            mutation RegisterUser($request: CreateUserRequestInput!) {
                registerUser(request: $request) {
                    token
                }
            }";

        var variables = new
        {
            request = new
            {
                username = "user1",
                email = email,
                password = "Password123!"
            }
        };

        await GraphQLHelper.ExecuteMutationAsync<RegisterUserResponse>(_httpClient, mutation, variables);

        var variables2 = new
        {
            request = new
            {
                username = "user2",
                email = email,
                password = "Password123!"
            }
        };

        var response = await GraphQLHelper.ExecuteMutationAsync(_httpClient, mutation, variables2);

        var content = await response.Content.ReadAsStringAsync();
        Assert.True(
            content.Contains("error", StringComparison.OrdinalIgnoreCase) || 
            content.Contains("already exists", StringComparison.OrdinalIgnoreCase) ||
            content.Contains("duplicate", StringComparison.OrdinalIgnoreCase) ||
            response.StatusCode == System.Net.HttpStatusCode.BadRequest,
            "Expected error for duplicate email");
    }

    [Fact]
    public async Task LoginUser_WithValidCredentials_ShouldReturnToken()
    {
        var email = $"logintest_{Guid.NewGuid():N}@example.com";
        var password = "SecurePassword123!";

        var registerMutation = @"
            mutation RegisterUser($request: CreateUserRequestInput!) {
                registerUser(request: $request) {
                    token
                }
            }";

        var registerVariables = new
        {
            request = new
            {
                username = $"loginuser_{Guid.NewGuid():N}",
                email = email,
                password = password
            }
        };

        await GraphQLHelper.ExecuteMutationAsync<RegisterUserResponse>(
            _httpClient, registerMutation, registerVariables);

        var loginMutation = @"
            mutation LoginUser($request: AuthenticateUserRequestInput!) {
                loginUser(request: $request) {
                    token
                    expireTime
                }
            }";

        var loginVariables = new
        {
            request = new
            {
                email = email,
                password = password
            }
        };

        var response = await GraphQLHelper.ExecuteMutationAsync<LoginUserResponse>(
            _httpClient, loginMutation, loginVariables);

        Assert.NotNull(response);
        Assert.Null(response.Errors);
        Assert.NotNull(response.Data?.LoginUser);
        Assert.NotNull(response.Data.LoginUser.Token);
    }

    [Fact]
    public async Task LoginUser_WithInvalidCredentials_ShouldReturnError()
    {
        var mutation = @"
            mutation LoginUser($request: AuthenticateUserRequestInput!) {
                loginUser(request: $request) {
                    token
                }
            }";

        var variables = new
        {
            request = new
            {
                email = "nonexistent@example.com",
                password = "WrongPassword123!"
            }
        };

        var response = await GraphQLHelper.ExecuteMutationAsync(_httpClient, mutation, variables);

        var content = await response.Content.ReadAsStringAsync();
        Assert.True(
            content.Contains("error", StringComparison.OrdinalIgnoreCase) || 
            response.StatusCode == System.Net.HttpStatusCode.BadRequest,
            "Expected error for invalid credentials");
    }
}
