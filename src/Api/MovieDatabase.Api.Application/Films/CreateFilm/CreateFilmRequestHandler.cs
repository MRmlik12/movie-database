using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Documents;
using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Films.CreateFilm;

public class CreateFilmRequestHandler(IFilmRepository filmRepository) : IRequestHandler<CreateFilmRequest, FilmDto>
{
    public async Task<FilmDto> HandleAsync(CreateFilmRequest request)
    {
        // var filmExists = await filmRepository.GetByName(request.Title) != null;
        // if (filmExists)
        // {
        //     throw new FilmExistApplicationException();
        // }
        //
        // var genreExists = await dispatcher.Dispatch(new VerifyGenreCreatedRequest(request.Genre.Id, request.Genre.Name));
        
        var film = new Film
        {
            Title = request.Title,
            ReleaseDate = request.ReleaseDate,
            Description = request.Description,
            Actors = request.Actors.Select(a => new Actor(a.Id, a.Name, a.Surname)).ToList(),
            Genres = request.Genres.Select(g => new Genre(g.Id, g.Name)).ToList(),
            Director = new DirectorInfo(request.Director.Id, request.Director.Name, request.Director.Surname),
            Producer = new ProducerInfo(request.Producer.Id, request.Producer.Name)
        };

        await filmRepository.Add(film);

        return FilmDto.From(film);
    }
}
