using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

using HotChocolate.Authorization;

using MovieDatabase.Api.Application.Films.CreateFilm;
using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api.Mutations;

[ExtendObjectType("Mutation")]
public class FilmMutations
{
    [Authorize]
    public async Task<FilmDto> CreateFilm(ClaimsPrincipal claimsPrincipal, CreateFilmRequest input, [Service] IDispatcher dispatcher)
    {
        var userId = claimsPrincipal.FindFirst(JwtRegisteredClaimNames.Sub);
        var userRole = claimsPrincipal.FindFirst(ClaimTypes.Role);
        var result = await dispatcher.Dispatch(input);

        return result;
    }
}