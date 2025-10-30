using MovieDatabase.Api.Application.Users.AuthenticateUser;
using MovieDatabase.Api.Application.Users.CreateUser;
using MovieDatabase.IntegrationTests.Fixtures;
using MovieDatabase.IntegrationTests.Helpers;
using MovieDatabase.IntegrationTests.Responses.Users;

using Shouldly;

namespace MovieDatabase.IntegrationTests.Mutations;

[Collection("AspireAppHost")]
public class UserMutationTests(AspireAppHostFixture fixture) : IClassFixture<AspireAppHostFixture>
{
    private readonly HttpClient _httpClient = fixture.CreateHttpClient("movies-db-api");

    [Fact]
    public async Task RegisterUser_WithValidData_ShouldReturnCredentials()
    {
        const string mutation = """
                                    mutation RegisterUser($request: CreateUserRequestInput!) {
                                        registerUser(request: $request) {
                                            token
                                            expireTime
                                        }
                                    }
                                """;

        var request = new CreateUserRequest(
            $"testuser_{Guid.NewGuid():N}",
            $"test_{Guid.NewGuid():N}@example.com",
            "SecurePassword123!"
        );

        var variables = new { request };

        var response = await GraphQLHelper.ExecuteMutationAsync<RegisterUserResponse>(
            _httpClient, mutation, variables);

        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
        response.Data.ShouldNotBeNull();
        response.Data.RegisterUser.Token.ShouldNotBeNull();
        response.Data.RegisterUser.ExpireTime.ShouldNotBeNull();
        (response.Data.RegisterUser.ExpireTime > DateTime.UtcNow).ShouldBeTrue();
    }

    [Fact]
    public async Task RegisterUser_WithDuplicateEmail_ShouldReturnError()
    {
        var email = $"duplicate_{Guid.NewGuid():N}@example.com";

        const string mutation = """
                                    mutation RegisterUser($request: CreateUserRequestInput!) {
                                        registerUser(request: $request) {
                                            token
                                        }
                                    }
                                """;

        var request1 = new CreateUserRequest(
            "user1",
            email,
            "Password123!"
        );

        await GraphQLHelper.ExecuteMutationAsync<RegisterUserResponse>(_httpClient, mutation, new { request = request1 });

        var request2 = new CreateUserRequest(
            "user2",
            email,
            "Password123!"
        );

        var response = await GraphQLHelper.ExecuteMutationAsync(_httpClient, mutation, new { request = request2 });

        var content = await response.Content.ReadAsStringAsync();
        var hasError = content.Contains("error", StringComparison.OrdinalIgnoreCase) ||
                       content.Contains("already exists", StringComparison.OrdinalIgnoreCase) ||
                       content.Contains("duplicate", StringComparison.OrdinalIgnoreCase) ||
                       response.StatusCode == System.Net.HttpStatusCode.BadRequest;
        hasError.ShouldBeTrue("Expected error for duplicate email");
    }

    [Fact]
    public async Task LoginUser_WithValidCredentials_ShouldReturnToken()
    {
        var email = $"logintest_{Guid.NewGuid():N}@example.com";
        const string password = "SecurePassword123!";

        const string registerMutation = """
                                            mutation RegisterUser($request: CreateUserRequestInput!) {
                                                registerUser(request: $request) {
                                                    token
                                                }
                                            }
                                        """;

        var registerRequest = new CreateUserRequest(
            $"loginuser_{Guid.NewGuid():N}",
            email,
            password
        );

        await GraphQLHelper.ExecuteMutationAsync<RegisterUserResponse>(
            _httpClient, registerMutation, new { request = registerRequest });

        const string loginMutation = """
                                         mutation LoginUser($request: AuthenticateUserRequestInput!) {
                                             loginUser(request: $request) {
                                                 token
                                                 expireTime
                                             }
                                         }
                                     """;

        var loginRequest = new AuthenticateUserRequest(
            email,
            password
        );

        var response = await GraphQLHelper.ExecuteMutationAsync<LoginUserResponse>(
            _httpClient, loginMutation, new { request = loginRequest });

        response.ShouldNotBeNull();
        response.Errors.ShouldBeNull();
        response.Data.ShouldNotBeNull();
        response.Data.LoginUser.ShouldNotBeNull();
        response.Data.LoginUser.Token.ShouldNotBeNull();
    }

    [Fact]
    public async Task LoginUser_WithInvalidCredentials_ShouldReturnError()
    {
        const string mutation = """
                                    mutation LoginUser($request: AuthenticateUserRequestInput!) {
                                        loginUser(request: $request) {
                                            token
                                        }
                                    }
                                """;

        var request = new AuthenticateUserRequest(
            "nonexistent@example.com",
            "WrongPassword123!"
        );

        var response = await GraphQLHelper.ExecuteMutationAsync(_httpClient, mutation, new { request });

        var content = await response.Content.ReadAsStringAsync();
        var hasError = content.Contains("error", StringComparison.OrdinalIgnoreCase) ||
                       response.StatusCode == System.Net.HttpStatusCode.BadRequest;
        hasError.ShouldBeTrue("Expected error for invalid credentials");
    }
}