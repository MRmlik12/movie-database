using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Films.GetDirectors;

public class GetDirectorsRequestHandler(IFilmRepository filmRepository) : IRequestHandler<GetDirectorsRequest, IEnumerable<DirectorDto>>
{
    public async Task<IEnumerable<DirectorDto>> HandleAsync(GetDirectorsRequest request)
    {
        var directors = await filmRepository.GetDirectors(request.Term);

        return directors.Select(DirectorDto.From);
    }
}