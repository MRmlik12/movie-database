namespace MovieDatabase.IntegrationTests.Responses.Actors;

public class ActorsResponse
{
    public ActorsConnection Actors { get; set; } = new();
}

public class ActorsConnection
{
    public List<ActorQueryDto> Nodes { get; set; } = new();
}

public class ActorQueryDto
{
    public string? Id { get; set; }
    public string? Name { get; set; }
    public string? Surname { get; set; }
}