using MovieDatabase.Api.Application.Films.Exceptions;
using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Films.DeleteFilm;

public class DeleteFilmRequestHandler(IFilmRepository filmRepository) : IRequestHandler<DeleteFilmRequest, string>
{
    public async Task<string> HandleAsync(DeleteFilmRequest request)
    {
        var film = await filmRepository.GetById(request.FilmId);

        if (film is null)
        {
            throw new FilmNotExistsApplicationException();
        }

        await filmRepository.Delete(film);

        return film.Id.ToString();
    }
}