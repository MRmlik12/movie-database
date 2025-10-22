namespace MovieDatabase.Api.Application.Films.CreateFilm;

public record CreateFilmInput(
    string Title,
    DateOnly ReleaseDate,
    string? Description,
    CreateFilmInput.ActorPlaceholder[] Actors,
    CreateFilmInput.GenrePlaceholder[] Genres,
    CreateFilmInput.DirectorPlaceholder Director,
    CreateFilmInput.ProducerPlaceholder Producer)
{
    public record ActorPlaceholder(string? Id, string? Name, string? Surname);

    public record GenrePlaceholder(string? Id, string? Name);

    public record DirectorPlaceholder(string? Id, string? Name, string? Surname);

    public record ProducerPlaceholder(string? Id, string? Name);
}