using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos.Users;
using MovieDatabase.Api.Core.Interfaces;

namespace MovieDatabase.Api.Application.Users.RevokeToken;

public sealed record RevokeTokenRequest(
    string AccessToken,
    string RefreshToken
) : IRequest<RevokeTokenDto>, IFrom<RevokeTokenRequest, RevokeTokenInput>
{
    public string? UserId { get; set; }
    
    public static RevokeTokenRequest From(RevokeTokenInput from)
        => new(from.AccessToken, from.RefreshToken);
}