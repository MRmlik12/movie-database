namespace MovieDatabase.IntegrationTests.Responses.Films;

public class FilmsConnection
{
    public List<FilmQueryDto> Nodes { get; set; } = new();
    public PageInfo? PageInfo { get; set; }
}
