using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Core.Interfaces;

namespace MovieDatabase.Api.Core.Dtos.Films;

public record FilmDto(
    string Id,
    string Title,
    string? Description,
    DateOnly ReleaseDate,
    ActorDto[] Actors,
    GenreDto[] Genres,
    ProducerDto Producer,
    DirectorDto Director
) : IFrom<FilmDto, Film>
{
    public static FilmDto From(Film document)
        => new(
            document.Id.ToString(),
            document.Title,
            document.Description,
            document.ReleaseDate,
            document.Actors.Select(ActorDto.From).ToArray(),
            document.Genres.Select(GenreDto.From).ToArray(),
            ProducerDto.From(document.Producer),
            DirectorDto.From(document.Director)
        );
}