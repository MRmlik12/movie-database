using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api.Application.Films.CreateFilm;

public record CreateFilmRequest(string Title, DateOnly ReleaseDate, string Description, ActorDto[] Actors, GenreDto[] Genres, DirectorDto Director, ProducerDto Producer) : IRequest<FilmDto>;
