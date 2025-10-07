using MovieDatabase.Api.Core.Documents;

namespace MovieDatabase.Api.Core.Dtos;

public record FilmDto(string Id, string Title, DateOnly ReleaseDate, ActorDto[] Actor, GenreDto Genre)
    : IFrom<FilmDto, Film>
{
    public static FilmDto From(Film document)
        => new (
            document.Id.ToString(),
            document.Title,
            document.ReleaseDate,
            document.Actors.Select(ActorDto.From).ToArray(),
            GenreDto.From(document.Genre)
        );
}