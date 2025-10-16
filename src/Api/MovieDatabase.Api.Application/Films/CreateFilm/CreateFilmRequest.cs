using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.Api.Application.Films.CreateFilm;



public record CreateFilmRequest(
    string Title,
    DateOnly ReleaseDate,
    string? Description,
    CreateFilmRequest.ActorPlaceholder[] Actors,
    CreateFilmRequest.GenrePlaceholder[] Genres,
    CreateFilmRequest.DirectorPlaceholder Director,
    CreateFilmRequest.ProducerPlaceholder Producer) : IRequest<FilmDto>
{
    public record ActorPlaceholder(string? Id, string? Name, string? Surname);

    public record GenrePlaceholder(string? Id, string? Name);

    public record DirectorPlaceholder(string? Id, string? Name, string? Surname);

    public record ProducerPlaceholder(string? Id, string? Name, string? Surname);

}