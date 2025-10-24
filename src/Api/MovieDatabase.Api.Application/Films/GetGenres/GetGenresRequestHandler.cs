using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Films.GetGenres;

public class GetGenresRequestHandler(IFilmRepository filmRepository) : IRequestHandler<GetGenresRequest, IEnumerable<GenreDto>>
{
    public async Task<IEnumerable<GenreDto>> HandleAsync(GetGenresRequest request)
    {
        var genres = await filmRepository.GetGenres(request.Term);

        return genres.Select(GenreDto.From).DistinctBy(x => x.Name);
    }
}