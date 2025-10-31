using MovieDatabase.Api.Core.Dtos.Users;

namespace MovieDatabase.IntegrationTests.Responses.Users;

public class RegisterUserResponse
{
    public UserCredentialsDto RegisterUser { get; set; } = null!;
}