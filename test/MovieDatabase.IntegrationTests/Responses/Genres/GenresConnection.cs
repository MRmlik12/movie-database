namespace MovieDatabase.IntegrationTests.Responses.Genres;

public class GenresConnection
{
    public List<GenreQueryDto> Nodes { get; set; } = new();
}
