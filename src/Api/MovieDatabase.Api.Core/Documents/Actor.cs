using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Core.Interfaces;

namespace MovieDatabase.Api.Core.Documents;

public class Actor : BaseDocument, IFrom<Actor, ActorDto>
{
    public string Name { get; set; }
    public string Surname { get; set; }

    public Actor(string id, string name, string surname)
    {
        Id = Guid.Parse(id);
        Name = name;
        Surname = surname;
    }

    public static Actor From(ActorDto document)
        => new (document.Id, document.Name, document.Surname);
}