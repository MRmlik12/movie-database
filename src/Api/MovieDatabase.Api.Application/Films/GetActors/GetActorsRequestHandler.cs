using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Films.GetActors;

public class GetActorsRequestHandler(IFilmRepository filmRepository) : IRequestHandler<GetActorsRequest, IEnumerable<ActorDto>>
{
    public async Task<IEnumerable<ActorDto>> HandleAsync(GetActorsRequest request)
    {
        var actors = await filmRepository.GetActors(request.SearchTerm);

        return actors.Select(ActorDto.From).ToArray();
    }
}