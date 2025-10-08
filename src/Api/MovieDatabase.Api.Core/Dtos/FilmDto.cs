using MovieDatabase.Api.Core.Documents;
using MovieDatabase.Api.Core.Interfaces;

namespace MovieDatabase.Api.Core.Dtos;

public record FilmDto(string Id, string Title, DateOnly ReleaseDate, ActorDto[] Actors, GenreDto[] Genres)
    : IFrom<FilmDto, Film>
{
    public static FilmDto From(Film document)
        => new (
            document.Id.ToString(),
            document.Title,
            document.ReleaseDate,
            document.Actors.Select(ActorDto.From).ToArray(),
            document.Genres.Select(GenreDto.From).ToArray()
        );
}