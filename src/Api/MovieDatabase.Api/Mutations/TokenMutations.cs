using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using HotChocolate.Authorization;

using MovieDatabase.Api.Application.Users.RevokeToken;
using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos.Users;

namespace MovieDatabase.Api.Mutations;

[ExtendObjectType("Mutation")]
public class TokenMutations
{
    [Authorize]
    public async Task<RevokeTokenDto> Revoke(ClaimsPrincipal claimsPrincipal, RevokeTokenInput input, [Service] IDispatcher dispatcher)
    {
        var userId = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Jti);
        
        var request = RevokeTokenRequest.From(input);
        request.UserId = userId?.Value;
        
        var result = await dispatcher.Dispatch(request);

        return result;
    }
}