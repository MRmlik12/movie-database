using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api.Application.Films.CreateFilm;

public record CreateFilmRequest(string Title, DateOnly ReleaseDate, ActorDto[] Actors, GenreDto Genre) : IRequest<FilmDto>;
