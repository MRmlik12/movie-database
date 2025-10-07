using MovieDatabase.Api.Core.Documents;

namespace MovieDatabase.Api.Core.Dtos;

public record GenreDto(string Id, string Name) : 
    IFrom<GenreDto, Genre>,
    IFrom<Genre, GenreDto>
{
    public static GenreDto From(Genre document)
        => new (document.Id.ToString(), document.Name);

    public static Genre From(GenreDto document)
        => new (document.Id, document.Name);
}