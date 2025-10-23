namespace MovieDatabase.Api.Application.Films.EditFilm;

public record EditFilmInput(
    string Id,
    string Title,
    DateOnly ReleaseDate,
    string? Description,
    EditFilmInput.EditFilmActorPlaceholder[] Actors,
    EditFilmInput.EditFilmGenrePlaceholder[] Genres,
    EditFilmInput.EditFilmDirectorPlaceholder Director,
    EditFilmInput.EditFilmProducerPlaceholder Producer)
{
    public record EditFilmActorPlaceholder(string? Id, string? Name, string? Surname);

    public record EditFilmGenrePlaceholder(string? Id, string? Name);

    public record EditFilmDirectorPlaceholder(string? Id, string? Name, string? Surname);

    public record EditFilmProducerPlaceholder(string? Id, string? Name);
}