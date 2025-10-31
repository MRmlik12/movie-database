using MovieDatabase.Api.Core.Dtos.Users;

namespace MovieDatabase.IntegrationTests.Responses.Users;

public class LoginUserResponse
{
    public UserCredentialsDto LoginUser { get; set; } = null!;
}