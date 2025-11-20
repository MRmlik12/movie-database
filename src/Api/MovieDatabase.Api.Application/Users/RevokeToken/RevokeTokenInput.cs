namespace MovieDatabase.Api.Application.Users.RevokeToken;

public sealed record RevokeTokenInput(string AccessToken, string RefreshToken);
