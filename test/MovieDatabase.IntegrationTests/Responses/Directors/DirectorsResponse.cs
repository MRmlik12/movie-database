using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.IntegrationTests.Responses.Directors;

public class DirectorsResponse
{
    public List<DirectorDto> Directors { get; set; } = new();
}