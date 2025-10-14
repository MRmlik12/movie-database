using MovieDatabase.Api.Core.Documents;
using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Core.Interfaces;

namespace MovieDatabase.Api.Core.Dtos;

public record GenreDto(string Id, string Name) : 
    IFrom<GenreDto, Genre>
{
    public static GenreDto From(Genre document)
        => new (document.Id.ToString(), document.Name);
}