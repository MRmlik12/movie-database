namespace MovieDatabase.Api.Core.Jwt;

public record JwtCredential(string Token, DateTime ExpireDate);
