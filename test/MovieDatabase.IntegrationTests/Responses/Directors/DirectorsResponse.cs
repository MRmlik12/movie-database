using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Core.Dtos.Films;

namespace MovieDatabase.IntegrationTests.Responses.Directors;

public class DirectorsResponse
{
    public List<DirectorDto> Directors { get; set; } = new();
}