using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Documents.Users;
using MovieDatabase.Api.Core.Dtos.Users;
using MovieDatabase.Api.Core.Exceptions.Auth;
using MovieDatabase.Api.Core.Services;
using MovieDatabase.Api.Infrastructure.Db;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Users.RevokeToken;

public sealed class RevokeTokenRequestHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IJwtService jwtService
) : IRequestHandler<RevokeTokenRequest, RevokeTokenDto>
{
    public async Task<RevokeTokenDto> HandleAsync(RevokeTokenRequest request)
    {
        var user = await userRepository.FindUserToRevokeToken(request.UserId, request.AccessToken, request.RefreshToken);
        if (user is null)
        {
            throw new TokenCannotBeRevokedApplicationException();
        }
        
        var credentials = jwtService.GenerateJwtToken(user);

        user.Tokens.Find(x => x.AccessToken == request.AccessToken && x.RefreshToken == request.RefreshToken)!.IsRevoked = true;
        user.Tokens.Add(new ClaimToken
        {
            AccessToken = credentials.AccessToken.Token,
            RefreshToken = credentials.RefreshToken.Token,
            ExpiresAt = credentials.RefreshToken.ExpireDate,
            IsRevoked = false
        });
        
        await unitOfWork.Commit();

        return RevokeTokenDto.From(credentials);
    }
}