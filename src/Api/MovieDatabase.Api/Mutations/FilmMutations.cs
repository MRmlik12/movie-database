using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using HotChocolate.Authorization;

using MovieDatabase.Api.Application.Films.CreateFilm;
using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Documents.Users;
using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api.Mutations;

[ExtendObjectType("Mutation")]
public class FilmMutations
{
    [Authorize(Roles = [nameof(UserRoles.Administrator)])]
    public async Task<FilmDto> CreateFilm(ClaimsPrincipal claimsPrincipal, CreateFilmInput input, [Service] IDispatcher dispatcher)
    {
        var userId = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Jti);

        var request = CreateFilmRequest.From(input);
        request.CreatorId = userId?.Value;

        var result = await dispatcher.Dispatch(request);

        return result;
    }
}