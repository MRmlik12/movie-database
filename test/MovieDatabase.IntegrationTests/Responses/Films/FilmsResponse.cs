using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.IntegrationTests.Responses.Films;

public class FilmsResponse
{
    public List<FilmQueryDto> Films { get; set; } = new();
}

// Wrapper for GraphQL response that matches the query structure
public class FilmQueryDto
{
    public string? Id { get; set; }
    public string? Title { get; set; }
    public string? Description { get; set; }
    public string? ReleaseDate { get; set; }
    public List<ActorDto>? Actors { get; set; }
    public DirectorDto? Director { get; set; }
    public List<GenreDto>? Genres { get; set; }
    public ProducerDto? Producer { get; set; }
}