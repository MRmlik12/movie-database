using MovieDatabase.Api.Core.Dtos;

namespace MovieDatabase.IntegrationTests.Responses.Actors;

public class ActorsResponse
{
    public List<ActorDto> Actors { get; set; } = new();
}