using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Documents;
using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Infrastructure.Db.Repositories;

namespace MovieDatabase.Api.Application.Films.CreateFilm;

internal class CreateFilmRequestHandler(IFilmRepository filmRepository, IDispatcher dispatcher) : IRequestHandler<CreateFilmRequest, FilmDto>
{
    public async Task<FilmDto> Handle(CreateFilmRequest request)
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
            Actors = request.Actors.Select(Actor.From).ToList(),
            Genres = request.Genres.Select(Genre.From).ToList(),
            Director = Director.From(request.Director),
            Producer = Producer.From(request.Producer)
        };

        await filmRepository.Add(film);

        return FilmDto.From(film);
    }
}
