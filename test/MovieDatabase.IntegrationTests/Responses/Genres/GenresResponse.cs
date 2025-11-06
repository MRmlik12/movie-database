using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Core.Dtos.Films;

namespace MovieDatabase.IntegrationTests.Responses.Genres;

public class GenresResponse
{
    public List<GenreDto> Genres { get; set; } = new();
}