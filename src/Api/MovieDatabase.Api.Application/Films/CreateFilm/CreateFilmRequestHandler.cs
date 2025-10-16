using MovieDatabase.Api.Application.Films.Exceptions;
using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Films.CreateFilm;

public class CreateFilmRequestHandler(IFilmRepository filmRepository) : IRequestHandler<CreateFilmRequest, FilmDto>
{
    public async Task<FilmDto> HandleAsync(CreateFilmRequest request)
    {
        var filmTitle = request.Title.TrimStart().TrimEnd();
        var existingFilm = await filmRepository.GetByName(filmTitle);
        if (existingFilm is not null)
        {
            throw new FilmExistsApplicationException();
        }

        var film = new Film
        {
            Title = filmTitle,
            ReleaseDate = request.ReleaseDate,
            Description = request.Description?.TrimStart().TrimEnd() ?? string.Empty,
            Actors = request.Actors.Select(a => new Actor(a.Id, a.Name, a.Surname)).ToList(),
            Genres = request.Genres.Select(g => new Genre(g.Id, g.Name)).ToList(),
            Director = new DirectorInfo(request.Director.Id, request.Director.Name, request.Director.Surname),
            Producer = new ProducerInfo(request.Producer.Id, request.Producer.Name)
        };

        await filmRepository.Add(film);

        return FilmDto.From(film);
    }
}