using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Films.GetFilms;

public class GetFilmsRequestHandler(IFilmRepository repository) : IRequestHandler<GetFilmsRequest, IEnumerable<FilmDto>>
{
    public async Task<IEnumerable<FilmDto>> HandleAsync(GetFilmsRequest request)
    {
        var films = await repository.GetAll(request.Title);

        return films.Select(FilmDto.From);
    }
}