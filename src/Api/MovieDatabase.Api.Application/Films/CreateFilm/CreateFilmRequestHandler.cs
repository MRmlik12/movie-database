using MovieDatabase.Api.Application.Films.Exceptions;
using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Documents;
using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Films.CreateFilm;

internal class CreateFilmRequestHandler(IFilmRepository filmRepository) : IRequestHandler<CreateFilmRequest, FilmDto>
{
    public async Task<FilmDto> Handle(CreateFilmRequest request)
    {
        var exists = await filmRepository.GetByName(request.Title) != null;
        if (exists)
        {
            throw new FilmExistApplicationException();
        }
        
        var film = new Film
        {
            Title = request.Title,
            ReleaseDate = request.ReleaseDate,
            Actors = request.Actors.Select(Actor.From).ToArray(),
            Genre = Genre.From(request.Genre) 
        };

        await filmRepository.Add(film);

        return FilmDto.From(film);
    }
}