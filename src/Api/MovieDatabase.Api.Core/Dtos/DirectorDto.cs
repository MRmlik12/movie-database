using MovieDatabase.Api.Core.Documents;
using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Core.Interfaces;

namespace MovieDatabase.Api.Core.Dtos;

public record DirectorDto(string Id, string Name, string Surname) : IFrom<DirectorDto, DirectorInfo>
{
    public static DirectorDto From(DirectorInfo document)
        => new (document.Id.ToString(), document.Name, document.Surname);
}
