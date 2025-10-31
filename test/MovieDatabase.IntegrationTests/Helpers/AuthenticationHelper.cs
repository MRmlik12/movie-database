using System.Net.Http.Headers;

using MovieDatabase.IntegrationTests.Responses.Users;

namespace MovieDatabase.IntegrationTests.Helpers;

public static class AuthenticationHelper
{
    public static async Task<string> RegisterAndLoginUserAsync(HttpClient client, string role = "User")
    {
        var email = $"testuser_{Guid.NewGuid():N}@example.com";
        var password = "SecurePassword123!";
        var username = $"user_{Guid.NewGuid():N}";

        var registerMutation = @"
            mutation RegisterUser($request: CreateUserRequestInput!) {
                registerUser(request: $request) {
                    token
                }
            }";

        var variables = new
        {
            request = new
            {
                username = username,
                email = email,
                password = password
            }
        };

        var response = await GraphQLHelper.ExecuteMutationAsync<RegisterUserResponse>(
            client, registerMutation, variables);

        return response?.Data?.RegisterUser?.Token
            ?? throw new Exception("Failed to register user");
    }

    public static async Task<string> LoginAsAdminAsync(HttpClient client)
    {
        var loginMutation = @"
            mutation LoginUser($request: AuthenticateUserRequestInput!) {
                loginUser(request: $request) {
                    token
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

        var response = await GraphQLHelper.ExecuteMutationAsync<LoginUserResponse>(
            client, loginMutation, variables);

        return response?.Data?.LoginUser?.Token
            ?? throw new Exception("Failed to login as admin");
    }

    public static void SetAuthorizationHeader(HttpClient client, string token)
    {
        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}