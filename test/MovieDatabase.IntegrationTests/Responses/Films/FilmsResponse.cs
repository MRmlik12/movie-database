namespace MovieDatabase.IntegrationTests.Responses.Films;

public class FilmsResponse
{
    public FilmsConnection Films { get; set; } = new();
}
