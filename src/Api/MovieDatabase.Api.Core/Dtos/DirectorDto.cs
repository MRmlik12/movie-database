using MovieDatabase.Api.Core.Documents;
using MovieDatabase.Api.Core.Interfaces;

namespace MovieDatabase.Api.Core.Dtos;

public record DirectorDto(string Id, string Name, string Surname) : IFrom<DirectorDto, Director>
{
    public static DirectorDto From(Director document)
        => new (document.Id.ToString(), document.Name, document.Surname);
}
