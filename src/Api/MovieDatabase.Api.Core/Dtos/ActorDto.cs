using MovieDatabase.Api.Core.Documents.Films;
using MovieDatabase.Api.Core.Interfaces;

namespace MovieDatabase.Api.Core.Dtos;

public record ActorDto(string Id, string Name, string Surname) :
    IFrom<ActorDto, Actor>,
    IFrom<Actor, ActorDto>
{
    public static ActorDto From(Actor document)
        => new(document.Id.ToString(), document.Name, document.Surname);

    public static Actor From(ActorDto document)
        => new()
        {
            Id = Guid.Parse(document.Id),
            Name = document.Name,
            Surname = document.Surname
        };
}