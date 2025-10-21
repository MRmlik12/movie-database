using MovieDatabase.Api.Core.Cqrs;
using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Core.Interfaces;

namespace MovieDatabase.Api.Application.Films.CreateFilm;



public record CreateFilmRequest(
    string Title,
    DateOnly ReleaseDate,
    string? Description,
    CreateFilmRequest.ActorPlaceholder[] Actors,
    CreateFilmRequest.GenrePlaceholder[] Genres,
    CreateFilmRequest.DirectorPlaceholder Director,
    CreateFilmRequest.ProducerPlaceholder Producer) : IRequest<FilmDto>, IFrom<CreateFilmRequest, CreateFilmInput>
{
    public record ActorPlaceholder(string? Id, string? Name, string? Surname);

    public record GenrePlaceholder(string? Id, string? Name);

    public record DirectorPlaceholder(string? Id, string? Name, string? Surname);

    public record ProducerPlaceholder(string? Id, string? Name);

    public string CreatorId { get; set; } = string.Empty;

    public static CreateFilmRequest From(CreateFilmInput from)
        => new CreateFilmRequest(
            from.Title,
            from.ReleaseDate,
            from.Description,
            from.Actors.Select(a => new ActorPlaceholder(a.Id, a.Name, a.Surname)).ToArray(),
            from.Genres.Select(g => new GenrePlaceholder(g.Id, g.Name)).ToArray(),
            new DirectorPlaceholder(from.Director.Id, from.Director.Name, from.Director.Surname),
            new ProducerPlaceholder(from.Producer.Id, from.Producer.Name)
        );
}