using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Core.Dtos.Films;

namespace MovieDatabase.IntegrationTests.Responses.Actors;

public class ActorsResponse
{
    public List<ActorDto> Actors { get; set; } = new();
}