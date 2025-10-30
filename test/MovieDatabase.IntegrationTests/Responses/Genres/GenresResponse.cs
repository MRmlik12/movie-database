using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.IntegrationTests.Responses.Genres;

public class GenresResponse
{
    public List<GenreDto> Genres { get; set; } = new();
}