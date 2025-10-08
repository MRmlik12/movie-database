using MovieDatabase.Api.Core.Dtos;
using MovieDatabase.Api.Core.Interfaces;

namespace MovieDatabase.Api.Core.Documents;

public class Director : BaseDocument, IFrom<Director, DirectorDto>
{
    private Director(string id, string name, string surname)
    {
        Id = Guid.Parse(id);
        Name = name;
        Surname = surname;
    }

    public string Name { get; set; } = null!;
    public string Surname { get; set; } = null!;

    public static Director From(DirectorDto document)
        => new(document.Id, document.Name, document.Surname);
}